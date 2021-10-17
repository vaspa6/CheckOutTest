using CheckOutTest.Core.BankManagement.Interfaces;
using CheckOutTest.Data.BDO;
using CheckOutTest.Data.DTO;
using CheckOutTest.Data.Helper;
using System;
using System.Threading.Tasks;

namespace CheckOutTest.Core.MockBank
{
    public class MockBank : IBank
    {
        public Task<ProcessPaymentBankResponseDto> ProcessPayment(PaymentBdo payment)
        {
            return Task.FromResult(
                payment.Currency switch
                {
                    "EUR" =>
                        new ProcessPaymentBankResponseDto { Id = Guid.NewGuid(), Status = PaymentStatusEnum.Successful },
                    "GBP" =>
                        new ProcessPaymentBankResponseDto { Id = Guid.NewGuid(), Status = PaymentStatusEnum.Failed },
                    _ =>
                         new ProcessPaymentBankResponseDto { Id = Guid.NewGuid(), Status = PaymentStatusEnum.Pending }
                });
        }
    }
}