using Learnix.Data;
using Learnix.Dtos.StudentDtos;
using Learnix.Models;
using Learnix.Repoisatories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;

namespace Learnix.Repoisatories.Implementations
{
    public class EnrollementRepository : GenericRepository<Enrollment,int>, IEnrollementRepository
    {
        public EnrollementRepository(LearnixContext context) : base(context) { }

        public IEnumerable<Student> GetAllCourseStudents(int courseId,string? search = null,int pageIndex = 1,int pageSize = 10)
        {
            /*
             
            var students = _dbSet
            .Where(e => e.CourseId == courseId)
            .Include(e => e.Student)
                .ThenInclude(s => s.User)
            .AsNoTracking()
            .Select(e => e.Student)
            .ToList();

            return students;

             
             */

            
            var query = _dbSet
                .Where(e => e.CourseId == courseId)
                .Include(e => e.Student)
                    .ThenInclude(s => s.User)
                .AsNoTracking()
                .Select(e => e.Student);

            
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                query = query.Where(s =>
                    s.User.FirstName.ToLower().Contains(search) ||
                    s.User.LastName.ToLower().Contains(search) ||
                    s.User.Email.ToLower().Contains(search));
            }

           
            return query
                .OrderBy(s => s.Id) 
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

        }

        public bool IsStudentEnrolled(string studentId, int courseId)
        {
            return _context.Enrollments
                .Any(e => e.StudentId == studentId && e.CourseId == courseId);
        }
    }
}


/*
 
public IEnumerable<StudentDto> GetAllCourseStudents(
    int courseId,
    string? search = null,
    int pageIndex = 1,
    int pageSize = 10)
{
    var query = _dbSet
        .Where(e => e.CourseId == courseId)
        .Select(e => new
        {
            e.Student.Id,
            e.Student.User.FirstName,
            e.Student.User.LastName,
            e.Student.User.Email
        });

    if (!string.IsNullOrEmpty(search))
    {
        search = search.ToLower();
        query = query.Where(s =>
            s.FirstName.ToLower().Contains(search) ||
            s.LastName.ToLower().Contains(search) ||
            s.Email.ToLower().Contains(search));
    }

    var result = query
        .OrderBy(s => s.Id) // Ensure deterministic ordering for pagination
        .Skip((pageIndex - 1) * pageSize)
        .Take(pageSize)
        .ToList();

    // Map to DTO
    return result.Select(s => new StudentDto
    {
        Id = s.Id,
        FirstName = s.FirstName,
        LastName = s.LastName,
        Email = s.Email
    });
}

 
 
 
 */
