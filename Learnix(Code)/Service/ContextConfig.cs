using Learnix.Data;
using Learnix.Dtos.AdminDtos;
using Learnix.Models;
using Learnix.Repoisatories.Implementations;
using Learnix.Services.Implementations;
using Learnix.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace UI.Services
{
    public class ContextConfig
    {
        private static readonly string seedAdminEmail = "admin04";

        public static async Task SeedDataAsync(LearnixContext context,UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            await SeedUserAsync(userManager, roleManager);
        }

        private static async Task SeedUserAsync(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            // Ensure roles exist
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("Instructor"))
            {
                await roleManager.CreateAsync(new IdentityRole("Instructor"));
            }

            if (!await roleManager.RoleExistsAsync("Student"))
            {
                await roleManager.CreateAsync(new IdentityRole("Student"));
            }


            ////Ensure admin user exists
            //var adminEmail = seedAdminEmail;
            //var adminUser = await userManager.FindByEmailAsync(adminEmail);
            //if (adminUser == null)
            //{
            //    var id = Guid.NewGuid().ToString();
            //    adminUser = new ApplicationUser
            //    {
            //        Id = id,
            //        UserName = adminEmail,
            //        Email = "albheryahmed361@gmail.com",
            //        EmailConfirmed = true,
            //        FirstName = "Ahmed",
            //        LastName = "Moustafa",
            //    };
            //    var result = await userManager.CreateAsync(adminUser, "Ahmed@123");
            //    await userManager.AddToRoleAsync(adminUser, "Admin");

            //}

            /* var reviewerUser = await userManager.FindByEmailAsync("Student@gmail.com");
             if (reviewerUser == null)
             {
                 var id = Guid.NewGuid().ToString();
                 reviewerUser = new ApplicationUser
                 {
                     Id = id,
                     UserName = "Student@gmail.com",
                     Email = "Student@gmail.com",
                     EmailConfirmed = true,
                     FirstName = "Student",
                     LastName = "Student",
                 };
                 var result = await userManager.CreateAsync(reviewerUser, "admin123456");
                 await userManager.AddToRoleAsync(reviewerUser, "Student");
             }

             var operationUser = await userManager.FindByEmailAsync("Instructor@gmail.com");
             if (operationUser == null)
             {
                 var id = Guid.NewGuid().ToString();
                 operationUser = new ApplicationUser
                 {
                     Id = id,
                     UserName = "Instructor@gmail.com",
                     Email = "Instructor@gmail.com",
                     EmailConfirmed = true,
                     FirstName = "Instructor",
                     LastName = "Instructor",
                 };
                 var result = await userManager.CreateAsync(operationUser, "admin123456");
                 await userManager.AddToRoleAsync(operationUser, "Instructor");
             }*/


        }
    }
}
