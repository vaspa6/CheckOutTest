using CheckOutTest.Data.DTO;
using System;
using System.Threading.Tasks;

namespace CheckOutTest.Core.PaymentManagement.Interfaces
{
    public interface IPaymentManager
    {
        Task<string> ProcessPayment(ProcessPaymentRequestDto paymentDto);
        Task<PaymentResponseDto> GetPayment(Guid paymentId);
    }
}
