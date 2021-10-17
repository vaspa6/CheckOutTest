using CheckOutTest.Core.PaymentManagement.Interfaces;
using CheckOutTest.Data.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CheckOutTest.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IPaymentManager _paymentManager;

        public PaymentController(ILogger<PaymentController> logger, IPaymentManager paymentManager)
        {
            _logger = logger;
            _paymentManager = paymentManager;
        }

        [HttpGet("{paymentId}")]
        [ProducesResponseType(typeof(PaymentResponseDto), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> GetPayment(Guid paymentId)
        {
            try
            {
                var result = await _paymentManager.GetPayment(paymentId);
                return Ok(result);
            }
            catch (ArgumentNullException aex)
            {
                _logger.LogInformation(aex, aex.Message);
                return NotFound($"No payment found with {paymentId} Id");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest("Getting a payment has failed with an exception");
            }
        }

        [HttpPost("Process")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 401)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> ProcessPayment(ProcessPaymentRequestDto requestDto)
        {
            try
            {
                return Ok(await _paymentManager.ProcessPayment(requestDto));
            }
            catch (ArgumentNullException aex)
            {
                _logger.LogError(aex, aex.Message);
                return BadRequest("Processing a payment has failed with an exception");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest("Processing a payment has failed with an exception");
            }
        }
    }
}