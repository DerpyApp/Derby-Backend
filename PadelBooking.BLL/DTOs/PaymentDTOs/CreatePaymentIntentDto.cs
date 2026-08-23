using System.ComponentModel.DataAnnotations;

namespace PadelBooking.BLL.DTOs.PaymentDTOs
{
    public class CreatePaymentIntentDto
    {
        [Required]
        public int BookingId { get; set; }
    }
}
