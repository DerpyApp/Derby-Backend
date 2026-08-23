using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PadelBooking.BLL.DTOs.PaymentDTOs;
using PadelBooking.BLL.Services.Payment;

namespace PadelBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // POST: /api/payments/intent
        [HttpPost("intent")]
        [Authorize]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentIntentDto dto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }

            var result = await _paymentService.CreatePaymentIntentAsync(dto.BookingId, userId);
            return Ok(result);
        }

        // GET: /api/payments/{id}
        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetPaymentDetails(int id)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }

            var result = await _paymentService.GetPaymentDetailsAsync(id, userId);
            return Ok(result);
        }

        // POST: /api/payments/webhooks/paymob
        [HttpPost("webhooks/paymob")]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> PaymobWebhook(
            [FromBody] PaymobWebhookPayloadDto payload,
            [FromQuery] string? hmac)
        {
            if (string.IsNullOrEmpty(hmac))
            {
                hmac = Request.Query["hmac"].ToString();
            }

            if (string.IsNullOrEmpty(hmac) && Request.Headers.TryGetValue("hmac", out var headerHmac))
            {
                hmac = headerHmac.ToString();
            }

            var success = await _paymentService.ProcessWebhookAsync(payload, hmac ?? string.Empty);
            return Ok(new { success });
        }
    }
}
