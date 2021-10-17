using System.Data;
using System.Threading.Tasks;
using System;
using CheckOutTest.Data.Configuration;

namespace CheckOutTest.Data.Repositories.Payment
{
    public class PaymentRepo : IPaymentRepo
    {
        private readonly DatabaseContext _dbContext;

        public PaymentRepo(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> AddPayment(Entities.Payment payment)
        {
            var result = await _dbContext.Payments.AddAsync(payment);
            if (result.State != Microsoft.EntityFrameworkCore.EntityState.Added)
            {
                throw new DataMisalignedException($"Insert of payment with id {payment.ID} has failed");
            }
            await _dbContext.SaveChangesAsync();
            return (payment.ID);
        }

        public async Task<Entities.Payment> GetPaymentById(Guid id)
        {
            var result = await _dbContext.Payments.FindAsync(id);

            return result;
        }
    }
}