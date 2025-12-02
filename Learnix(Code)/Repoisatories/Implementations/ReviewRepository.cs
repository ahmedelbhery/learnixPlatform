using Learnix.Data;
using Learnix.Models;
using Learnix.Repoisatories.Interfaces;

namespace Learnix.Repoisatories.Implementations
{
    public class ReviewRepository : GenericRepository<Review,int>, IReviewRepository
    {
        public ReviewRepository(LearnixContext context) : base(context) { }
    }
}
