using CheckOutTest.Core.PaymentManagement.Interfaces;
using CheckOutTest.Data.DTO;
using CheckOutTest.Data.Helper;
using CheckOutTest.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CheckOutTest.Web.Test
{
    public class PaymentControllerTest
    {
        private readonly Mock<ILogger<PaymentController>> _mockLogger;
        private readonly Mock<IPaymentManager> _mockPaymentManager;

        public PaymentControllerTest()
        {
            _mockLogger = new Mock<ILogger<PaymentController>>();
            _mockPaymentManager = new Mock<IPaymentManager>();
        }

        [Fact]
        public async Task GetPaymentTest_Returns200()
        {
            var mockResult = new PaymentResponseDto()
            {
                CardNumber = "1234567856321234",
                Amount = 120,
                Currency = "GBP",
                ExpiryDate = "12/23"
            };

            _mockPaymentManager
                .Setup(x => x.GetPayment(It.IsAny<Guid>()))
                .Returns(Task.FromResult(mockResult));

            var controller = new PaymentController(_mockLogger.Object, _mockPaymentManager.Object);

            var result = await controller.GetPayment(Guid.NewGuid());

            var viewResult = Assert.IsType<OkObjectResult>(result);

            var model = Assert.IsAssignableFrom<PaymentResponseDto>(viewResult.Value);

            Assert.Equal(mockResult, model);
        }

        [Fact]
        public async Task GetPaymentTest_Returns404()
        {
            var paymentGuid = Guid.NewGuid();

            _mockPaymentManager
                .Setup(x => x.GetPayment(It.IsAny<Guid>()))
                .ThrowsAsync(new ArgumentNullException("Mock exception"));

            var controller = new PaymentController(_mockLogger.Object, _mockPaymentManager.Object);

            var result = await controller.GetPayment(paymentGuid);

            var viewResult = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal($"No payment found with {paymentGuid} Id", viewResult.Value);
        }

        [Fact]
        public async Task GetPaymentTest_Returns500()
        {
            var paymentGuid = Guid.NewGuid();

            _mockPaymentManager
                .Setup(x => x.GetPayment(It.IsAny<Guid>()))
                .ThrowsAsync(new Exception("Mock exception"));

            var controller = new PaymentController(_mockLogger.Object, _mockPaymentManager.Object);

            var result = await controller.GetPayment(paymentGuid);

            var viewResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal("Getting a payment has failed with an exception", viewResult.Value);
        }

        [Fact]
        public async Task ProcessPaymentTest_Returns200()
        {
            var mockResult = new ProcessPaymentRequestDto()
            {
                CardNumber = "1234567856321234",
                Amount = 120,
                Currency = "GBP",
                ExpiryDate = "12/23",
                CVV = 895
            };

            _mockPaymentManager
                .Setup(x => x.ProcessPayment(It.IsAny<ProcessPaymentRequestDto>()))
                .Returns(Task.FromResult(PaymentStatusEnum.Successful.ToString()));

            var controller = new PaymentController(_mockLogger.Object, _mockPaymentManager.Object);

            var result = await controller.ProcessPayment(mockResult);

            var viewResult = Assert.IsType<OkObjectResult>(result);

            var model = Assert.IsAssignableFrom<string>(viewResult.Value);

            Assert.Equal(PaymentStatusEnum.Successful.ToString(), model);
        }

        [Fact]
        public async Task ProcessPaymentTest_Returns500_NullEx()
        {
            _mockPaymentManager
                .Setup(x => x.ProcessPayment(It.IsAny<ProcessPaymentRequestDto>()))
                .ThrowsAsync(new ArgumentNullException("Mock exception"));

            var controller = new PaymentController(_mockLogger.Object, _mockPaymentManager.Object);

            var result = await controller.ProcessPayment(null);

            var viewResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal("Processing a payment has failed with an exception", viewResult.Value);
        }

        [Fact]
        public async Task ProcessPaymentTest_Returns500_Ex()
        {
            _mockPaymentManager
                .Setup(x => x.ProcessPayment(It.IsAny<ProcessPaymentRequestDto>()))
                .ThrowsAsync(new Exception("Mock exception"));

            var controller = new PaymentController(_mockLogger.Object, _mockPaymentManager.Object);

            var result = await controller.ProcessPayment(null);

            var viewResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal("Processing a payment has failed with an exception", viewResult.Value);
        }
    }
}