using System.Threading.Tasks;
using PadelBooking.BLL.DTOs.PaymentDTOs;

namespace PadelBooking.BLL.Services.Payment
{
    public interface IPaymentService
    {
        Task<PaymentIntentResponseDto> CreatePaymentIntentAsync(int bookingId, int userId);
        Task<PaymentDetailsDto> GetPaymentDetailsAsync(int paymentId, int userId);
        Task<bool> ProcessWebhookAsync(PaymobWebhookPayloadDto payload, string hmac);
    }
}
