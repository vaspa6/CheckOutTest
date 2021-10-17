using System;
using System.Threading.Tasks;

namespace CheckOutTest.Data.Repositories.Payment
{
    public interface IPaymentRepo
    {
        Task<Guid> AddPayment(Entities.Payment payment);
        Task<Entities.Payment> GetPaymentById(Guid id);
    }
}