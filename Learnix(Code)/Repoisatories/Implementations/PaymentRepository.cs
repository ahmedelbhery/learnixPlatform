using Learnix.Data;
using Learnix.Models;
using Learnix.Repoisatories.Interfaces;

namespace Learnix.Repoisatories.Implementations
{
    public class PaymentRepository : GenericRepository<Payment,int>, IPaymentRepository
    {
        public PaymentRepository(LearnixContext context) : base(context) { }
    }
}
