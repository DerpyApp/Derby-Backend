namespace PadelBooking.API.DTOs
{
    public class UserFilterDto
    {
        public string? SearchTerm { get; set; }

        // الفلترة بدور معين: Player أو ClubOwner
        public string? Role { get; set; }
    }
}
