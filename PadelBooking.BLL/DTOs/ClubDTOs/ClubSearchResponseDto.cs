namespace PadelBooking.BLL.DTOs.ClubDTOs
{
    public class ClubSearchResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Address { get; set; } = null!;
        public string? City { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public double? DistanceKm { get; set; }
        public decimal? StartingPrice { get; set; }
        public string? Logo { get; set; }
        public string? CoverImage { get; set; }
    }
}
