using Learnix.Dtos.InstructorDtos;
using Learnix.Models;
using Microsoft.EntityFrameworkCore;

namespace Learnix.Services.Interfaces
{
    public interface IInstructorService : IGenericService<Instructor, InstructorDto,string>
    {
        Task<InstructorMainDashboardDto> MainDashboard(string id);
        public double GetTotalRevenueForInstructor(string instructorId);
        
    }
}
