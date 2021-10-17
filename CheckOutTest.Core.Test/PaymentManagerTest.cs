using AutoMapper;
using CheckOutTest.Core.BankManagement.Interfaces;
using CheckOutTest.Core.PaymentManagement;
using CheckOutTest.Data.BDO;
using CheckOutTest.Data.DTO;
using CheckOutTest.Data.Entities;
using CheckOutTest.Data.Helper;
using CheckOutTest.Data.Repositories.Payment;
using CheckOutTest.Web;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Xunit;

namespace CheckOutTest.Core.Test
{
    public class PaymentManagerTest
    {
        private readonly PaymentManager _paymentManager;
        private readonly Mock<ILogger<PaymentManager>> _mockLogger;
        private readonly IMapper _mockMapper;
        private readonly Mock<IBank> _mockBank;
        private readonly Mock<IPaymentRepo> _mockPaymentRepo;

        public PaymentManagerTest()
        {
            var mappConfig = new MapperConfiguration(opts =>
            {
                opts.AddProfile<MappingProfile>();
            });

            _mockLogger = new Mock<ILogger<PaymentManager>>();
            _mockMapper = mappConfig.CreateMapper();
            _mockPaymentRepo = new Mock<IPaymentRepo>();
            _mockBank = new Mock<IBank>();

            _paymentManager = new PaymentManager(
                _mockLogger.Object,
                _mockMapper,
                _mockBank.Object,
                _mockPaymentRepo.Object);
        }

        [Fact]
        public async Task GetPaymentTest_Success()
        {
            _mockPaymentRepo
                .Setup(x => x.GetPaymentById(It.IsAny<Guid>()))
                .Returns(Task.FromResult(new Payment()
                {
                    ID = Guid.NewGuid(),
                    CardNumber = "1234567891234567",
                    ExpiryDate = "02/24",
                    Currency = "GBP",
                    Amount = 123,
                    Created = DateTime.ParseExact("14/11/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture)
                }));

            var result = await _paymentManager.GetPayment(Guid.NewGuid());

            //Test mapping
            Assert.IsType<PaymentResponseDto>(result);
            Assert.Equal(123, result.Amount);
            Assert.Equal("xxxx-xxxx-xxxx-4567", result.CardNumber);
            Assert.Equal("GBP", result.Currency);
            Assert.Equal("02/24", result.ExpiryDate);
        }

        [Fact]
        public async Task GetPaymentTest_Exception()
        {
            var guid = Guid.NewGuid();
            _mockPaymentRepo
                .Setup(x => x.GetPaymentById(It.IsAny<Guid>()))
                .Returns(Task.FromResult((Payment)null));

            var result = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _paymentManager.GetPayment(guid));
            Assert.Equal($"Payment with {guid} is not found in data store (Parameter 'payment')", result.Message);
        }

        [Fact]
        public async Task ProcessPaymentTest_Success_BankSuccessful()
        {
            _mockBank
                .Setup(x => x.ProcessPayment(It.IsAny<PaymentBdo>()))
                .Returns(Task.FromResult(new ProcessPaymentBankResponseDto()
                {
                    Id = Guid.NewGuid(),
                    Status = PaymentStatusEnum.Successful
                }));

            var result = await _paymentManager.ProcessPayment(new ProcessPaymentRequestDto()
            {
                CardNumber = "1234567891234567",
                ExpiryDate = "02/24",
                Currency = "GBP",
                Amount = 123,
                CVV = 895
            });

            Assert.Equal(PaymentStatusEnum.Successful.ToString(), result);
        }

        [Fact]
        public async Task ProcessPaymentTest_Success_BankFailed()
        {
            _mockBank
                .Setup(x => x.ProcessPayment(It.IsAny<PaymentBdo>()))
                .Returns(Task.FromResult(new ProcessPaymentBankResponseDto()
                {
                    Id = Guid.NewGuid(),
                    Status = PaymentStatusEnum.Failed
                }));

            var result = await _paymentManager.ProcessPayment(new ProcessPaymentRequestDto()
            {
                CardNumber = "1234567891234567",
                ExpiryDate = "02/24",
                Currency = "GBP",
                Amount = 123,
                CVV = 895
            });

            Assert.Equal(PaymentStatusEnum.Failed.ToString(), result);
        }

        [Fact]
        public async Task ProcessPaymentTest_Success_BankPending()
        {
            _mockBank
                .Setup(x => x.ProcessPayment(It.IsAny<PaymentBdo>()))
                .Returns(Task.FromResult(new ProcessPaymentBankResponseDto()
                {
                    Id = Guid.NewGuid(),
                    Status = PaymentStatusEnum.Pending
                }));

            var result = await _paymentManager.ProcessPayment(new ProcessPaymentRequestDto()
            {
                CardNumber = "1234567891234567",
                ExpiryDate = "02/24",
                Currency = "GBP",
                Amount = 123,
                CVV = 895
            });

            Assert.Equal(PaymentStatusEnum.Pending.ToString(), result);
        }

        [Fact]
        public async Task ProcessPaymentTest_Fail_DtoException()
        {
            _mockBank
                .Setup(x => x.ProcessPayment(It.IsAny<PaymentBdo>()))
                .Returns(Task.FromResult(new ProcessPaymentBankResponseDto()
                {
                    Id = Guid.NewGuid(),
                    Status = PaymentStatusEnum.Successful
                }));

            var result = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _paymentManager.ProcessPayment(null));
            Assert.Equal("Request object is null (Parameter 'ProcessPaymentRequestDto')", result.Message);
        }

        [Fact]
        public async Task ProcessPaymentTest_Fail_BankException()
        {
            _mockBank
                .Setup(x => x.ProcessPayment(It.IsAny<PaymentBdo>()))
                .Returns(Task.FromResult((ProcessPaymentBankResponseDto)null));

            var result = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _paymentManager.ProcessPayment(new ProcessPaymentRequestDto()
            {
                CardNumber = "1234567891234567",
                ExpiryDate = "02/24",
                Currency = "GBP",
                Amount = 123,
                CVV = 895
            }));
            Assert.Equal("Response from the bank provider is null (Parameter 'paymentResponse')", result.Message);
        }
    }
}