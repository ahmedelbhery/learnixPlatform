using Learnix.Models;
using Microsoft.EntityFrameworkCore;

namespace Learnix.Repoisatories.Interfaces
{
    public interface IInstructorRepository : IGenericRepository<Instructor,string>
    {
        public double GetTotalRevenueForInstructor(string instructorId);
        

    }
}
