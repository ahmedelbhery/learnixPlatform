using Learnix.Dtos.ReviewDtos;
using Learnix.Models;

namespace Learnix.Services.Interfaces
{
    public interface IReviewService : IGenericService<Review, ReviewDto,int>
    {
    }
}
