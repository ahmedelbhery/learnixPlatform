using Learnix.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Learnix.Data
{
    public class LearnixContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseLanguage> CourseLanguages { get; set; }
        public DbSet<CourseLevel> CourseLevels { get; set; }
        public DbSet<CourseStatus> CourseStatuses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<LessonMaterial> LessonMaterials { get; set; }
        public DbSet<LessonStatus> LessonStatuses { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Student> Students  { get; set; }
        public DbSet<StudentLessonProgress> StudentLessonProgresses { get; set; }
        public DbSet<InstructorStatus> InstructorStatus { get; set; }

        public LearnixContext(DbContextOptions options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>()
                .Property(s => s.Balance)
                .HasDefaultValue(500);

            modelBuilder.Entity<Student>()
                .Property(s => s.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<Student>().ToTable(tb =>
               tb.HasCheckConstraint("CK_Student_Balance", "[Balance] >= 0"));





            modelBuilder.Entity<Instructor>()
                .Property(i => i.Balance)
                .HasDefaultValue(0);

            modelBuilder.Entity<Instructor>()
                .Property(I => I.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<Instructor>().ToTable(tb =>
                tb.HasCheckConstraint("CK_Instructor_Balance", "[Balance] >= 0"));





            modelBuilder.Entity<Admin>()
               .Property(a => a.Balance)
               .HasDefaultValue(0);

            modelBuilder.Entity<Admin>()
                .Property(a => a.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<Admin>().ToTable(tb =>
               tb.HasCheckConstraint("CK_Admin_Balance", "[Balance] >= 0"));




            
            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(u => u.Email)
                .IsUnique();


            modelBuilder.Entity<ApplicationUser>()
                .HasIndex(u => u.NormalizedEmail)
                .IsUnique();



            



        }


    }
}


