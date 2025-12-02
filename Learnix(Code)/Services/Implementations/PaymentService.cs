using Learnix.Dtos.PaymentsDtos;
using Learnix.Models;
using Learnix.Repoisatories.Interfaces;
using Learnix.Services.Interfaces;

namespace Learnix.Services.Implementations
{
    public class PaymentService : GenericService<Payment,PaymentDto,int> , IPaymentService
    {
        public PaymentService(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
