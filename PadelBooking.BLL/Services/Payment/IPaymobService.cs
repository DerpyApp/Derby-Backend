using System.Threading.Tasks;
using PadelBooking.BLL.DTOs.PaymentDTOs;

namespace PadelBooking.BLL.Services.Payment
{
    public interface IPaymobService
    {
        Task<(string ClientSecret, string IntentionId, string CheckoutUrl)> CreateIntentionAsync(
            decimal amount,
            string currency,
            string specialReference,
            string firstName,
            string lastName,
            string email,
            string phoneNumber);

        bool VerifyHmacSignature(PaymobTransactionObjectDto transaction, string receivedHmac);
    }
}
