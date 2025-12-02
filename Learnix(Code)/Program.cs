using Learnix.Data;
using Learnix.Dtos.CategoryDtos;
using Learnix.Dtos.CourseLanguageDtos;
using Learnix.Dtos.CourseLevelDtos;
using Learnix.Dtos.CourseStatusDtos;
using Learnix.Models;
using Learnix.Others;
using Learnix.Repoisatories.Implementations;
using Learnix.Repoisatories.Interfaces;
using Learnix.Services.Implementations;
using Learnix.Services.Interfaces;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UI.Services;

namespace Learnix
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.AddScoped<IEmailService, EmailService>();




            builder.Services.AddDbContext<LearnixContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IImageService, ImageService>();
            builder.Services.AddScoped<IVideoService, VideoService>();
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IAdminRepository, AdminRepository>();
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<IEnrollementService, EnrollementService>();
            builder.Services.AddScoped<IInstructorService, InstructorService>();
            builder.Services.AddScoped<ILessonMaterialsService, LessonMaterialsService>();
            builder.Services.AddScoped<ILessonService, LessonService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<ISectionService, SectionService>();
            builder.Services.AddScoped<IStudentLessonProgressService, StudentLessonProgressService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped(typeof(IGenericService<,,>), typeof(GenericService<,,>));


            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true; 

            }).AddEntityFrameworkStores<LearnixContext>();


            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 500 * 1024 * 1024; // 500 MB
            });

            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 500 * 1024 * 1024; // 500 MB
            });















            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();   // Must be BEFORE routing
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseStaticFiles();     // Serve files from wwwroot FIRST
            app.MapStaticAssets();    // Then map static assets



            app.MapControllerRoute(
               name: "areas",
               pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}")
               .WithStaticAssets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

           
           


            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var dbContext = services.GetRequiredService<LearnixContext>();

                // Apply migrations
                await dbContext.Database.MigrateAsync();

                // Seed data
                await ContextConfig.SeedDataAsync(dbContext, userManager, roleManager);
            }


            app.Run();
        }
    }
}
