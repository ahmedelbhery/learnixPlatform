using Learnix.Dtos.ReviewDtos;
using Learnix.Models;
using Learnix.Repoisatories.Interfaces;
using Learnix.Services.Interfaces;

namespace Learnix.Services.Implementations
{
    public class ReviewService : GenericService<Review,ReviewDto,int> , IReviewService
    {
        public ReviewService(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
