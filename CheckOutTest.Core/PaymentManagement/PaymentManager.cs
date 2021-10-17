using AutoMapper;
using CheckOutTest.Core.BankManagement.Interfaces;
using CheckOutTest.Core.PaymentManagement.Interfaces;
using CheckOutTest.Data.BDO;
using CheckOutTest.Data.DTO;
using CheckOutTest.Data.Entities;
using CheckOutTest.Data.Helper;
using CheckOutTest.Data.Repositories.Payment;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CheckOutTest.Core.PaymentManagement
{
    public class PaymentManager : IPaymentManager
    {
        private protected ILogger<PaymentManager> _logger;
        private protected IMapper _mapper;
        private protected IBank _bank;
        private protected IPaymentRepo _paymentRepo;


        public PaymentManager(ILogger<PaymentManager> logger, IMapper mapper, IBank bank, IPaymentRepo paymentRepo)
        {
            _logger = logger;
            _mapper = mapper;
            _bank = bank;
            _paymentRepo = paymentRepo;
        }

        public async Task<ProcessPaymentResponseDto> ProcessPayment(ProcessPaymentRequestDto paymentDto)
        {
            if (paymentDto == null)
            {
                throw new ArgumentNullException("ProcessPaymentRequestDto", "Request object is null");
            }
            var paymentBdo = _mapper.Map<PaymentBdo>(paymentDto);
            var paymentResponse = await _bank.ProcessPayment(paymentBdo);

            if (paymentResponse == null)
            {
                throw new ArgumentNullException("paymentResponse", "Response from the bank provider is null");
            }
            if (paymentResponse.Status == PaymentStatusEnum.Successful)
            {
                var payment = _mapper.Map<Payment>(paymentBdo);
                payment.ID = paymentResponse.Id;
                payment.Created = DateTime.UtcNow;
                try
                {
                    var dbResult = await _paymentRepo.AddPayment(payment);
                }
                catch (DataMisalignedException dmex)
                {
                    _logger.LogError($"Saving payment with {paymentResponse.Id} has failed", dmex);
                }
            }
            _logger.LogInformation($"Payment with {paymentResponse.Id} Id was processed with the following status: {paymentResponse.Status} ");

            return new ProcessPaymentResponseDto() { Id = paymentResponse.Id, Status = paymentResponse.Status.ToString()};
        }

        public async Task<PaymentResponseDto> GetPayment(Guid paymentId)
        {
            var payment = await _paymentRepo.GetPaymentById(paymentId);
            if (payment == null)
            {
                throw new ArgumentNullException("payment", $"Payment with {paymentId} is not found in data store");
            }
            return _mapper.Map<PaymentResponseDto>(payment);
        }
    }
}