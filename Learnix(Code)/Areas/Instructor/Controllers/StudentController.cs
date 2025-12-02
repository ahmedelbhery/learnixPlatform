using Learnix.Models;
using Learnix.Others;
using Learnix.Services.Interfaces;
using Learnix.ViewModels.UserVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Learnix.Areas.Instructor.Controllers
{
    [Area("Instructor")]
    [Authorize(Roles = "Instructor")]
    public class StudentController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentController(IEmailService emailService, UserManager<ApplicationUser> userManager)
        {
            _emailService = emailService;
            _userManager = userManager;
        }

        public async Task<IActionResult> SendMessage(string StudentEmail,string CourseName,int CourseID ,string MessageText)
        {
            Claim IDClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var Instructor = await _userManager.FindByIdAsync(IDClaim.Value);

            var InstructorName = Instructor.FirstName + " " + Instructor.LastName;

            Email email = new Email()
            {
                Subject = $"Message from your Instructor/{InstructorName} regarding {CourseName}",
                Body = MessageText,
                ReceiverEmail = StudentEmail
                
            };


            await _emailService.SendEmailAsync(email);

            TempData["SuccessMessage"] = "Message Sended successfully!";

            
            return RedirectToAction(
                actionName: "DisplayCourseStudents",
                controllerName: "Course",
                routeValues: new { area = "Instructor", id = CourseID }
            );

        }

        public async Task<IActionResult> DispalyProfile(string id,int CrsID)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return View("NotFound");

            var vm = new DisplayUserInfoVM()
            {
                ID = id,
                FullName = user.FirstName + " " + user.LastName,
                ImageUrl = user.ImageUrl,
                LinkedinUrl = user.Linkedin,
                Bio = user.Bio,
                Location = user.Address,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                CourseID = CrsID

            };

            return View("DispalyProfile", vm);
        }
    }
}
