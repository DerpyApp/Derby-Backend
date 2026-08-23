namespace PadelBooking.BLL.DTOs.ClubDTOs
{
    public class ClubFilterRequestDto
    {
        public string? Sport { get; set; }
        public string? City { get; set; }
        public string? SearchTerm { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
