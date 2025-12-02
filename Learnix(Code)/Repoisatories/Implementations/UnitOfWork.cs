using Learnix.Data;
using Learnix.Repoisatories.Implementations;
using Learnix.Repoisatories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Learnix.Repoisatories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LearnixContext _context;
        private readonly Dictionary<Type, object> _repositories = new();

        public IAnnouncementRepository Announcements { get; private set; }

        public ICategoryRepository Categories { get; private set; }

        public ICourseRepository Courses { get; private set; }

        public IEnrollementRepository Enrollements { get; private set; }

        public IInstructorRepository Instructors { get; private set; }

        public ILessonMaterialsRepository LessonMaterials { get; private set; }

        public ILessonRepository Lessons { get; private set; }

        public IPaymentRepository Payments { get; private set; }

        public IReviewRepository Reviews { get; private set; }

        public ISectionRepository Sections { get; private set; }

        public IStudentLessonProgressRepository StudentLessonsProgress { get; private set; }

        public IStudentRepository Students { get; private set; }

        public IAdminRepository Admins { get; private set; }

        public UnitOfWork(LearnixContext context)
        {
            _context = context;
            Announcements = new AnnouncementRepository(_context);
            Categories = new CategoryRepository(_context);
            Courses = new CourseRepository(_context);
            Enrollements = new EnrollementRepository(_context);
            Instructors = new InstructorRepository(_context);
            LessonMaterials = new LessonMaterialsRepository(_context);
            Lessons = new LessonRepository(_context);
            Payments = new PaymentRepository(_context);
            Reviews = new ReviewRepository(_context);
            Sections = new SectionRepository(_context);
            Students = new StudentRepository(_context);
            Admins = new AdminRepository(_context);
            StudentLessonsProgress = new StudentLessonProgressRepository(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericRepository<TEntity,DT> Repository<TEntity,DT>() where TEntity : class
        {
            if (!_repositories.ContainsKey(typeof(TEntity)))
            {
                var repo = new GenericRepository<TEntity,DT>(_context);
                _repositories.Add(typeof(TEntity), repo);
            }

            return (IGenericRepository<TEntity,DT>)_repositories[typeof(TEntity)];
        }

        public IDbContextTransaction BeginTransaction()
       => _context.Database.BeginTransaction();
    }
}

