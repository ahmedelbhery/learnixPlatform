using Learnix.Dtos.PaymentsDtos;
using Learnix.Models;

namespace Learnix.Services.Interfaces
{
    public interface IPaymentService : IGenericService<Payment, PaymentDto,int>
    {
    }
}
