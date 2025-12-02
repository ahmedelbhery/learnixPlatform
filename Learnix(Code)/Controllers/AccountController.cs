using Learnix.Dtos.AdminDtos;
using Learnix.Dtos.InstructorDtos;
using Learnix.Dtos.StudentDtos;
using Learnix.Models;
using Learnix.Others;
using Learnix.Services.Implementations;
using Learnix.Services.Interfaces;
using Learnix.ViewModels.AccountVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace Learnix.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IAccountService accountService;
        private readonly IEmailService emailService;
        private readonly IStudentService _studentService;
        private readonly IAdminService _adminService;
        private readonly IInstructorService _instructorService;
        private readonly IImageService _imageService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IAccountService accountService, IEmailService emailService,
            IStudentService StudentService, IInstructorService InstructorService, IImageService ImageService, IAdminService adminService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.accountService = accountService;
            this.emailService = emailService;
            this._studentService = StudentService;
            this._instructorService = InstructorService;
            this._imageService = ImageService;
            this._adminService = adminService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }


        [HttpPost]
        public async Task<IActionResult> SaveRegister(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await userManager.FindByNameAsync(registerVM.UserName);
                if (existingUser != null)
                {
                    ModelState.AddModelError("UserName", "This username is already taken.");
                    return View("Register",registerVM);
                }

                var existingUser2 = await userManager.FindByEmailAsync(registerVM.Email);
                if (existingUser2 != null)
                {
                    ModelState.AddModelError("Email", "This email is already in use.");
                    return View("Register", registerVM);
                }

                var UserAccount = new ApplicationUser();
                UserAccount.Email = registerVM.Email;
                UserAccount.UserName = registerVM.UserName;
                UserAccount.FirstName = registerVM.FirstName;
                UserAccount.LastName = registerVM.LastName;

                var result = await userManager.CreateAsync(UserAccount, registerVM.Password);

                if (result.Succeeded)
                {


                    //    await userManager.AddToRoleAsync(UserAccount, "Admin");
                    //var Admin = new AdminDto()
                    //{
                    //    Id = UserAccount.Id,
                    //};


                    //_adminService.Add(Admin);




                    if (registerVM.RoleName == "Student")
                    {

                        await userManager.AddToRoleAsync(UserAccount, registerVM.RoleName);

                    }

                    //Cookie
                    //await signInManager.SignInAsync(UserAccount, false);

                    if (registerVM.RoleName == "Student")
                    {
                        var Student = new StudentDto()
                        {
                            Id = UserAccount.Id,
                        };

                        //accountService.StudentService.Add(Student);
                        _studentService.Add(Student);

                    }

                    else if (registerVM.RoleName == "Instructor")
                    {
                        var Instructor = new InstructorDto()
                        {
                            Id = UserAccount.Id,
                            //Status = 1 ===> Pending
                            StatusId = 1
                        };

                        //accountService.InstructorService.Add(Instructor);
                        _instructorService.Add(Instructor);
                    }


                    // Sending an Email after Registeration
                    Email email = new Email();
                    email.ReceiverEmail = registerVM.Email;
                    email.Subject = "Welcome to Learnix";
                    email.Body = $@"
Hello {registerVM.FirstName} {registerVM.LastName},

Welcome to Learnix! 🎉

Your account has been successfully created. We’re excited to have you join our learning community.

You can now log in, and start your journey:

If you have any questions or need help, feel free to reach out to our support team at learnixelearningplatform@gmail.com.

Best regards,  
The Learnix Team
";
                    await emailService.SendEmailAsync(email);


                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);

                    return View("Register", registerVM);

                }

            }

            else
            {
                return View("Register", registerVM);
            }

        }


        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }


        [HttpPost]
        public async Task<IActionResult> SaveLogin(LoginVM loginVM)
        {
            if (ModelState.IsValid == true)
            {
                //check found 
                ApplicationUser appUser = await userManager.FindByNameAsync(loginVM.UserName);
                if (appUser != null)
                {
                    bool found =
                         await userManager.CheckPasswordAsync(appUser, loginVM.Password);
                    if (found == true)
                    {
                        
                        
                        await signInManager.SignInAsync(appUser, loginVM.RememberMe);
                        return RedirectToAction("DisplayProfile");

                    }

                }

                ModelState.AddModelError("", "Username OR Password is Wrong");
                
            }

            return View("Login", loginVM);
        }

        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return View("Login");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DisplayProfile()
        {
            ApplicationUser appUser = await userManager.FindByNameAsync(User.Identity.Name);

            if (appUser == null)
            {
                return View("NotFound");
            }
            else
            {
                var RoleName = User.FindFirstValue(ClaimTypes.Role);

                ProfileVM profileVM = new ProfileVM()
                {
                    ID = appUser.Id,
                    FirstName = appUser.FirstName,
                    LastName = appUser.LastName,
                    Email = appUser.Email,
                    RoleName = RoleName,
                    ImageUrl = appUser.ImageUrl,
                    Bio = appUser.Bio,
                    Gender = appUser.Gender,
                    Address = appUser.Address,
                    Linkedin = appUser.Linkedin,
                    DOB = appUser.DOB,
                    PhoneNumber = appUser.PhoneNumber,
                };

               /* if(RoleName == "Instructor")
                {

                    //InstructorDto instructor = accountService.InstructorService.GetById(appUser.Id);
                }*/

                return View("Profile",profileVM);

            }
            
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ProfileVM profileVM)
        {
            if(ModelState.IsValid)
            {
                var existingUser = await userManager.FindByEmailAsync(profileVM.Email);
                if (existingUser != null && existingUser.Id != profileVM.ID)
                {
                    ModelState.AddModelError("Email", "This email is already in use.");
                    return View("Profile", profileVM);
                }

                ApplicationUser appUser = await userManager.FindByIdAsync(profileVM.ID);

                if (appUser == null)
                {
                    return View("NotFound");
                }
                
                appUser.FirstName = profileVM.FirstName;
                appUser.LastName = profileVM.LastName;
                appUser.Email = profileVM.Email;
                appUser.Bio = profileVM.Bio;
                appUser.Gender = profileVM.Gender;
                appUser.Address = profileVM.Address;
                appUser.Linkedin = profileVM.Linkedin;
                appUser.PhoneNumber = profileVM.PhoneNumber;
                appUser.DOB = profileVM.DOB;


                var result = await userManager.UpdateAsync(appUser);

                if (result.Succeeded)
                {
                    return RedirectToAction("DisplayProfile");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View("Profile", profileVM);
            }
            else
            {
                return View("Profile",profileVM);
            }
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateAvatar(UpdateAvatarVM updateAvatar)
        {
            if(updateAvatar.Avatar==null)
            {
                TempData["ErrorMsg"] = "Please Upload a Photo";
                return RedirectToAction("DisplayProfile");
            }

            ApplicationUser appUser = await userManager.FindByNameAsync(User.Identity.Name);

            if (appUser == null)
            {
                return View("NotFound");
            }


            //accountService.ImageService.Delete(appUser.ImageUrl);
            _imageService.Delete(appUser.ImageUrl);

            //var ImagePath = accountService.ImageService.Upload(updateAvatar.Avatar,"Users");
            var ImagePath = _imageService.Upload(updateAvatar.Avatar, "Users");
            appUser.ImageUrl = ImagePath;
            var result = await userManager.UpdateAsync(appUser);

            return RedirectToAction("DisplayProfile");

        }


        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyEmail(string Email, string? ID)
        {
            var user = await userManager.FindByEmailAsync(Email);

            // case 1: Adding new user
            if (ID == null)
                return Json(user == null);

            // case 2: Updating existing user
            if (user != null && user.Id != ID)
                return Json(false);

            return Json(true);
        }



        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyUserName(string UserName, string? ID)
        {
            var user = await userManager.FindByNameAsync(UserName);

            // case 1: Adding new user
            if (ID == null)
                return Json(user == null);

            // case 2: Updating existing user
            if (user != null && user.Id != ID)
                return Json(false);

            return Json(true);
        }

        public IActionResult PendingAccountReview()
        {
            return View("PendingAccountReview");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View("Forbidden");
        }




    }
}



/*
 
 
 public IActionResult TestAuth()
        {
            if (User.Identity.IsAuthenticated == true)
            {
                
                Claim IDClaim= User.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

                Claim AddressClaim = User.Claims.FirstOrDefault(c => c.Type == "UserAddress");

                string id = IDClaim.Value;

                string name = User.Identity.Name;
                return Content($"welcome {name} \n ID= {id}");
            }
            return Content("Welcome Guest");
        }
 
 
 */



