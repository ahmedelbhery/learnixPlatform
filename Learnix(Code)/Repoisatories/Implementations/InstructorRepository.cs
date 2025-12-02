using Learnix.Data;
using Learnix.Models;
using Learnix.Repoisatories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Learnix.Repoisatories.Implementations
{
    public class InstructorRepository : GenericRepository<Instructor,string>, IInstructorRepository
    {
        public InstructorRepository(LearnixContext context) : base(context) { }

        public double GetTotalRevenueForInstructor(string instructorId)
        {
            var totalRevenue = _context.Courses
                               .Where(c => c.InstructorID == instructorId)
                               .Select(c => (c.Price ?? 0) * c.Enrollments.Count())
                               .Sum();

            return totalRevenue;
        }
    }
}

