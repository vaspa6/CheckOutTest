using CheckOutTest.Data.DTO;
using System;
using System.Threading.Tasks;

namespace CheckOutTest.Core.PaymentManagement.Interfaces
{
    public interface IPaymentManager
    {
        Task<ProcessPaymentResponseDto> ProcessPayment(ProcessPaymentRequestDto paymentDto);
        Task<PaymentResponseDto> GetPayment(Guid paymentId);
    }
}
