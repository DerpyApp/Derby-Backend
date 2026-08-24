namespace PadelBooking.BLL.DTOs.AdminDTOs
{
    public class BookingTrendDto
    {
        public DateTime Date { get; set; }

        public int TotalBookings { get; set; }

        public decimal Revenue { get; set; }
    }
}