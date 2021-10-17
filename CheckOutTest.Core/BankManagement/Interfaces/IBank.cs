using CheckOutTest.Data.BDO;
using CheckOutTest.Data.DTO;
using System.Threading.Tasks;

namespace CheckOutTest.Core.BankManagement.Interfaces
{
    public interface IBank
    {
        Task<ProcessPaymentBankResponseDto> ProcessPayment(PaymentBdo payment);
    }
}