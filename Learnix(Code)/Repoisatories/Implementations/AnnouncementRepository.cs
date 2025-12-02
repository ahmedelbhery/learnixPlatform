using Learnix.Data;
using Learnix.Models;
using Learnix.Repoisatories.Interfaces;

namespace Learnix.Repoisatories.Implementations
{
    public class AnnouncementRepository : GenericRepository<Announcement,int>, IAnnouncementRepository
    {
        public AnnouncementRepository(LearnixContext context) : base(context) { }
    }
}
