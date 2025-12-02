using Learnix.Dtos.AnnouncementDtos;
using Learnix.Models;
using Learnix.Repoisatories.Interfaces;
using Learnix.Services.Interfaces;

namespace Learnix.Services.Implementations
{
    public class AnnouncementService : GenericService<Announcement,AnnoucementDto,int>, IAnnouncementService
    {
        public AnnouncementService(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
