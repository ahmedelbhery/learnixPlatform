using Learnix.Repoisatories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Learnix.Repoisatories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity,DT> Repository<TEntity,DT>() where TEntity : class;
        IAnnouncementRepository Announcements { get; }
        ICategoryRepository Categories { get; }
        ICourseRepository Courses { get; }
        IEnrollementRepository Enrollements { get; }
        IInstructorRepository Instructors { get; }
        ILessonMaterialsRepository LessonMaterials { get; }
        ILessonRepository Lessons { get; }
        IPaymentRepository Payments { get; }
        IReviewRepository Reviews { get; }
        ISectionRepository Sections { get; }
        IStudentLessonProgressRepository StudentLessonsProgress { get; }
        IStudentRepository Students { get; }
        IAdminRepository Admins { get; }
        int Complete();
        IDbContextTransaction BeginTransaction();
    }
}

