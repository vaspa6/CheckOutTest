using System.Data;
using Dapper.Logging;
using System.Threading.Tasks;
using System;

namespace CheckOutTest.Data.Repositories.Payment
{
    public class PaymentRepo : IPaymentRepo
    {
        private protected IDbConnection _dbConnection;
        public PaymentRepo(IDbConnectionFactory dbConnection)
        {
            _dbConnection = dbConnection.CreateConnection();
        }

        public Task<bool> AddPayment(Entities.Payment payment)
        {
            using (IDbConnection conn = _dbConnection)
            {
                //SQL logic
            }
            return Task.FromResult(true);
        }

        public Task<Entities.Payment> GetPaymentById(Guid id)
        {
            using (IDbConnection conn = _dbConnection)
            {
                //SQL logic
            }
            return Task.FromResult(new Entities.Payment { CardNumber = "4444333311112456", ExpiryDate = "02/28", ID = Guid.NewGuid(), Amount = 10, Created = DateTime.UtcNow, Currency = "GBP" });
        }
    }
}