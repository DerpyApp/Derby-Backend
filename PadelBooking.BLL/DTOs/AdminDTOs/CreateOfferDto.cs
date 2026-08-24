using PadelBooking.DAL.Enums;

namespace PadelBooking.BLL.DTOs.AdminDTOs
{
    public class CreateOfferDto
    {
        public int ClubId { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DiscountType DiscountType { get; set; }

        public decimal DiscountValue { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}