namespace PadelBooking.BLL.DTOs.PaymentDTOs
{
    public class PaymentIntentResponseDto
    {
        public int PaymentId { get; set; }
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "EGP";
        public string ClientSecret { get; set; } = null!;
        public string PublicKey { get; set; } = null!;
        public string? IntentionId { get; set; }
        public string? CheckoutUrl { get; set; }
    }
}
