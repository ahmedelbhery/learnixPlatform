
using Learnix.Dtos.AdminDtos;
using Learnix.Models;
using Learnix.Others;
using Learnix.Repoisatories.Interfaces;
using Learnix.Services.Interfaces;
using Learnix.ViewModels.AccountVMs;
using Learnix.ViewModels.AdminVMs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Learnix.Services.Implementations
{
    public class AdminService : GenericService<Admin, AdminDto, string>, IAdminService
    {
        private readonly IEmailService _emailService;
        private readonly IAdminRepository _repo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IAdminRepository repo, IEmailService emailService)
            : base(unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _repo = repo;
            _emailService = emailService;
        }




        //User
        public async Task<PagedResult<AdminUserVM>> GetAllUsersAsync(string role, int pageNumber, int pageSize)
        {
            IQueryable<ApplicationUser> usersQuery = _userManager.Users;

            if (!string.IsNullOrEmpty(role) && role.ToLower() != "all")
            {
                var usersList = await usersQuery.ToListAsync();
                var filteredUsers = new List<ApplicationUser>();

                foreach (var user in usersList)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Any(r => r.Equals(role, StringComparison.OrdinalIgnoreCase)))
                    {
                        filteredUsers.Add(user);
                    }
                }

                var totalUsers = filteredUsers.Count;
                var pagedUsers = filteredUsers
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var userVMs = new List<AdminUserVM>();
                foreach (var user in pagedUsers)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    userVMs.Add(new AdminUserVM
                    {
                        Id = user.Id,
                        UserName = user.UserName ?? "",
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email ?? "",
                        RoleName = roles.FirstOrDefault() ?? "No Role",
                        ImageUrl = string.IsNullOrEmpty(user.ImageUrl)
                          ? "/Images/Default_User_Image.jpg"
                          : user.ImageUrl
                    });
                }

                return new PagedResult<AdminUserVM>
                {
                    Items = userVMs,
                    PageNumber = pageNumber,
                    TotalPages = (int)Math.Ceiling(totalUsers / (double)pageSize)
                };
            }
            else
            {

                var totalUsers = await usersQuery.CountAsync();

                var pagedUsers = await usersQuery
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var userVMs = new List<AdminUserVM>();
                foreach (var user in pagedUsers)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    userVMs.Add(new AdminUserVM
                    {
                        Id = user.Id,
                        UserName = user.UserName ?? "",
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email ?? "",
                        RoleName = roles.FirstOrDefault() ?? "No Role",
                        ImageUrl = string.IsNullOrEmpty(user.ImageUrl)
                          ? "/images/default-user.png"
                          : user.ImageUrl
                    });
                }

                return new PagedResult<AdminUserVM>
                {
                    Items = userVMs,
                    PageNumber = pageNumber,
                    TotalPages = (int)Math.Ceiling(totalUsers / (double)pageSize)
                };
            }
        }

        public async Task<AdminUserVM?> GetUserDetailsAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            return new AdminUserVM
            {
                Id = user.Id,
                UserName = user.UserName ?? "",
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? "",
                RoleName = roles.FirstOrDefault() ?? "No Role",
                ImageUrl = string.IsNullOrEmpty(user.ImageUrl)
                          ? "/Images/Default_User_Image.jpg"
                          : user.ImageUrl
            };
        }

        public async Task<(bool Success, string Message)> UpdateUserAsync(AdminUserVM vm)
        {
            var user = await _userManager.FindByIdAsync(vm.Id);
            if (user == null)
                return (false, "User not found.");

            user.UserName = vm.UserName;
            user.Email = vm.Email;
            user.FirstName = vm.FirstName;
            user.LastName = vm.LastName;
            if (!string.IsNullOrEmpty(vm.ImageUrl))
                user.ImageUrl = vm.ImageUrl;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return (false, string.Join("; ", result.Errors.Select(e => e.Description)));

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (!currentRoles.Contains(vm.RoleName))
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, vm.RoleName);
            }

            return (true, "User updated successfully.");
        }

        public async Task<(int totalUsers, int admins, int instructors, int students)> GetUserStatisticsAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            int totalUsers = users.Count;

            int admins = 0, instructors = 0, students = 0;

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Admin")) admins++;
                if (roles.Contains("Instructor")) instructors++;
                if (roles.Contains("Student")) students++;
            }

            return (totalUsers, admins, instructors, students);
        }





        //Instructor
        public async Task<PagedResult<AdminInstructorVM>> GetAllInstructorsAsync(string specialty, int pageNumber, int pageSize)
        {

            var instructorUsers = await _userManager.GetUsersInRoleAsync("Instructor");
            var instructorUserIds = instructorUsers.Select(u => u.Id).ToList();

            var query = await _repo.GetInstructorsWithUserAsync();
            query = query.Where(i => instructorUserIds.Contains(i.Id) && i.Status.Name == "Approved");



            if (!string.IsNullOrEmpty(specialty) && specialty.ToLower() != "all")
            {
                query = query.Where(i => i.Major.ToLower() == specialty.ToLower());
            }



            var instructorsList = await query.ToListAsync();


            var instructorVMs = new List<AdminInstructorVM>();
            foreach (var instructor in instructorsList)
            {
                var instructorId = instructor.Id;
                var coursesCount = await _repo.GetCoursesCountByInstructorIdsAsync(new List<string> { instructorId });
                var studentsCount = await _repo.GetStudentsCountByInstructorIdsAsync(new List<string> { instructorId });
                var earnings = await _repo.GetEarningsByInstructorIdsAsync(new List<string> { instructorId });

                instructorVMs.Add(new AdminInstructorVM
                {
                    Id = instructor.User.Id,
                    FirstName = instructor.User.FirstName,
                    LastName = instructor.User.LastName,
                    ImageUrl = instructor.User.ImageUrl,
                    Email = instructor.User.Email,
                    UserName = instructor.User.UserName,
                    Specialty = instructor.Major,
                    CoursesCount = coursesCount,
                    StudentsCount = studentsCount,
                    Earnings = Convert.ToDouble(earnings * (95m/100)),
                });
            }


            var totalCount = instructorVMs.Count;
            var pagedInstructors = instructorVMs
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<AdminInstructorVM>
            {
                Items = pagedInstructors,
                PageNumber = pageNumber,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }

        public async Task<AdminInstructorVM?> GetInstructorDetailsAsync(string id)
        {
            var instructor = await _repo.GetInstructorWithUserByIdAsync(id);
            if (instructor == null) return null;

            var instructorId = instructor.Id;
            var coursesCount = await _repo.GetCoursesCountByInstructorIdsAsync(new List<string> { instructorId });
            var studentsCount = await _repo.GetStudentsCountByInstructorIdsAsync(new List<string> { instructorId });
            var earnings = await _repo.GetEarningsByInstructorIdsAsync(new List<string> { instructorId });

            return new AdminInstructorVM
            {
                Id = instructor.Id,
                FirstName = instructor.User.FirstName,
                LastName = instructor.User.LastName,
                ImageUrl = instructor.User.ImageUrl,
                Email = instructor.User.Email,
                UserName = instructor.User.UserName,
                Specialty = instructor.Major,
                CoursesCount = coursesCount,
                StudentsCount = studentsCount,
                Earnings = Convert.ToDouble(earnings),
            };
        }

        public async Task<(bool Success, string Message)> UpdateInstructorAsync(AdminInstructorVM vm)
        {
            try
            {
                var instructor = await _repo.GetInstructorWithUserByIdAsync(vm.Id);
                if (instructor == null)
                    return (false, "Instructor not found.");


                instructor.User.FirstName = vm.FirstName;
                instructor.User.LastName = vm.LastName;
                instructor.User.Email = vm.Email;
                instructor.User.UserName = vm.UserName;
                if (!string.IsNullOrEmpty(vm.ImageUrl))
                    instructor.User.ImageUrl = vm.ImageUrl;


                instructor.Major = vm.Specialty;


                var result = await _userManager.UpdateAsync(instructor.User);
                if (!result.Succeeded)
                    return (false, string.Join("; ", result.Errors.Select(e => e.Description)));

                await _repo.UpdateInstructorAsync(instructor);

                return (true, "Instructor updated successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Error updating instructor: {ex.Message}");
            }
        }

        public async Task<(int totalInstructors, int totalCourses, int totalStudents, decimal totalEarnings)> GetInstructorStatisticsAsync()
        {
            var instructorUsers = await _userManager.GetUsersInRoleAsync("Instructor");
            var instructorUserIds = instructorUsers.Select(u => u.Id).ToList();

            var totalInstructors = instructorUserIds.Count;
            var totalCourses = await _repo.GetTotalCoursesCountAsync();
            var totalStudents = await _repo.GetTotalStudentsCountAsync();
            var totalEarnings = await _repo.GetTotalEarningsAsync();

            return (totalInstructors, totalCourses, totalStudents, totalEarnings);
        }

        public async Task<List<string>> GetSpecialtiesAsync()
        {
            return await _repo.GetInstructorSpecialtiesAsync();
        }

        public async Task<PagedResult<InstructorReviewVM>> GetPendingInstructorsAsync(int pageNumber, int pageSize)
        {
            var query = await _repo.GetPendingInstructorsWithUserAsync();
            var instructorsList = await query.ToListAsync();

            var instructorVMs = new List<InstructorReviewVM>();
            foreach (var instructor in instructorsList)
            {
                instructorVMs.Add(new InstructorReviewVM
                {
                    Id = instructor.Id,
                    UserName = instructor.User.UserName,
                    FullName = $"{instructor.User.FirstName} {instructor.User.LastName}",
                    Email = instructor.User.Email,
                    Major = instructor.Major,
                    Status = instructor.Status.Name,
                    ImageUrl = string.IsNullOrEmpty(instructor.User.ImageUrl)
                        ? "/images/default-user.png"
                        : instructor.User.ImageUrl,
                    Bio = instructor.User.Bio
                });
            }

            var totalCount = instructorVMs.Count;
            var pagedInstructors = instructorVMs
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<InstructorReviewVM>
            {
                Items = pagedInstructors,
                PageNumber = pageNumber,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }

        public async Task<(bool Success, string Message)> ApproveInstructorAsync(string instructorId)
        {
            try
            {
                var instructor = await _repo.GetInstructorWithUserByIdAsync(instructorId);
                if (instructor == null)
                    return (false, "Instructor not found.");

                // Get Approved status 
                var approvedStatus = await _repo.GetInstructorStatus("Approved");

                if (approvedStatus == null)
                    return (false, "Approved status not found in system.");

                instructor.StatusId = approvedStatus.Id;

                await _repo.UpdateInstructorAsync(instructor);

                var UserAccount = await _userManager.FindByIdAsync(instructorId);

                await _userManager.AddToRoleAsync(UserAccount, "Instructor");

                // Send approval email
                await SendApprovalEmail(instructor.User.Email, instructor.User.FirstName);

                return (true, "Instructor approved successfully and notification email sent.");
            }
            catch (Exception ex)
            {
                return (false, $"Error approving instructor: {ex.Message}");
            }
        }

        public async Task<(bool Success, string Message)> RejectInstructorAsync(string instructorId)
        {
            try
            {
                var instructor = await _repo.GetInstructorWithUserByIdAsync(instructorId);
                if (instructor == null)
                    return (false, "Instructor not found.");

                // Get Rejected status
                var rejectedStatus = await _repo.GetInstructorStatus("Rejected");

                if (rejectedStatus == null)
                    return (false, "Rejected status not found in system.");

                instructor.StatusId = rejectedStatus.Id;
                await _repo.UpdateInstructorAsync(instructor);

                // Send rejection email
                await SendRejectionEmail(instructor.User.Email, instructor.User.FirstName);

                return (true, "Instructor rejected successfully and notification email sent.");
            }
            catch (Exception ex)
            {
                return (false, $"Error rejecting instructor: {ex.Message}");
            }
        }

        private async Task SendApprovalEmail(string email, string firstName)
        {
            var emailModel = new Email
            {
                ReceiverEmail = email,
                Subject = "Instructor Account Approved - Learnix",
                Body = $@"
                         Dear {firstName},
                         
                         We are pleased to inform you that your instructor account has been approved! 🎉
                         
                         You can now log in to your account and start creating courses on Learnix.
                         
                         Welcome to our teaching community!
                         
                         Best regards,
                         The Learnix Team
                         "
            };

            await _emailService.SendEmailAsync(emailModel);
        }

        private async Task SendRejectionEmail(string email, string firstName)
        {
            var emailModel = new Email
            {
                ReceiverEmail = email,
                Subject = "Instructor Account Application Status - Learnix",
                Body = $@"
                         Dear {firstName},
                         
                         Thank you for your interest in becoming an instructor on Learnix.
                         
                         After careful review, we regret to inform you that your instructor application has not been approved at this time. 
                         
                         If you have any questions or would like more information, please don't hesitate to contact our support team.
                         
                         We appreciate your understanding.
                         
                         Best regards,
                         The Learnix Team
                         "
            };

            await _emailService.SendEmailAsync(emailModel);
        }





        // Courses
        public async Task<PagedResult<AdminCourseVM>> GetAllCoursesAsync(string status, string category, int pageNumber, int pageSize)
        {
            var query = await _repo.GetCoursesWithIncludesAsync();


            if (!string.IsNullOrEmpty(status) && status.ToLower() != "all")
            {
                query = query.Where(c => c.Status.Name.ToLower() == status.ToLower());
            }


            if (!string.IsNullOrEmpty(category) && category.ToLower() != "all")
            {
                query = query.Where(c => c.Category.Name.ToLower() == category.ToLower());
            }


            var coursesList = await query.ToListAsync();
            var courseVMs = new List<AdminCourseVM>();

            foreach (var course in coursesList)
            {
                var studentsCount = await _repo.GetEnrollmentsCountByCourseIdAsync(course.Id);
                var lessonsCount = await _repo.GetLessonsCountByCourseIdAsync(course.Id);
                var duration = await _repo.GetDurationSumByCourseIdAsync(course.Id);

                courseVMs.Add(new AdminCourseVM
                {
                    Id = course.Id,
                    Title = course.Title,
                    Description = course.Description,
                    ImageUrl = course.ImageUrl,
                    Price = course.Price,
                    InstructorName = course.Instructor.User.FirstName + " " + course.Instructor.User.LastName,
                    Category = course.Category.Name,
                    Language = course.Language.Name,
                    Level = course.Level.Name,
                    Status = course.Status.Name,
                    StudentsCount = studentsCount,
                    LessonsCount = lessonsCount,
                    Duration = duration
                });
            }

            var totalCount = courseVMs.Count;
            var pagedCourses = courseVMs
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<AdminCourseVM>
            {
                Items = pagedCourses,
                PageNumber = pageNumber,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
        }

        public async Task<AdminCourseVM?> GetCourseDetailsAsync(int id)
        {
            var course = await _repo.GetCourseWithIncludesByIdAsync(id);
            if (course == null) return null;

            var studentsCount = await _repo.GetEnrollmentsCountByCourseIdAsync(course.Id);
            var lessonsCount = await _repo.GetLessonsCountByCourseIdAsync(course.Id);
            var duration = await _repo.GetDurationSumByCourseIdAsync(course.Id);

            return new AdminCourseVM
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                ImageUrl = course.ImageUrl,
                Price = course.Price,
                InstructorName = course.Instructor.User.FirstName + " " + course.Instructor.User.LastName,
                Category = course.Category.Name,
                Language = course.Language.Name,
                Level = course.Level.Name,
                Status = course.Status.Name,
                StudentsCount = studentsCount,
                LessonsCount = lessonsCount,
                Duration = duration
            };
        }

        public async Task<(bool Success, string Message)> UpdateCourseAsync(AdminCourseVM vm)
        {
            try
            {
                var course = await _repo.GetCourseWithIncludesByIdAsync(vm.Id);
                if (course == null)
                    return (false, "Course not found.");


                course.Title = vm.Title;
                course.Description = vm.Description;
                course.Price = vm.Price;
                if (!string.IsNullOrEmpty(vm.ImageUrl))
                    course.ImageUrl = vm.ImageUrl;

                await _repo.UpdateCourseAsync(course);

                return (true, "Course updated successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Error updating course: {ex.Message}");
            }
        }

        public async Task<(int totalCourses, int publishedCourses, int pendingCourses, int totalEnrollments)> GetCourseStatisticsAsync()
        {
            var totalCourses = await _repo.GetCoursesCountAsync();
            var publishedCourses = await _repo.GetCoursesCountByStatusAsync("Publish");
            var pendingCourses = await _repo.GetCoursesCountByStatusAsync("Draft");
            var totalEnrollments = await _repo.GetEnrollmentsCountAsync();

            return (totalCourses, publishedCourses, pendingCourses, totalEnrollments);
        }

        public async Task<List<string>> GetCourseCategoriesAsync()
        {
            return await _repo.GetCourseCategoriesAsync();
        }

        public async Task<List<string>> GetCourseStatusesAsync()
        {
            return await _repo.GetCourseStatusesAsync();
        }
    }
}
