
using Learnix.Dtos.AnnouncementDtos;
using Learnix.Models;

namespace Learnix.Services.Interfaces
{
    public interface IAnnouncementService : IGenericService<Announcement, AnnoucementDto,int>
    {
    }
}
