namespace PadelBooking.DAL.Constants
{
    public static class AppRoles
    {
        public const string Player = "Player";
        public const string ClubOwner = "ClubOwner";
        public const string Admin = "Admin";

        public static readonly string[] All = { Player, ClubOwner, Admin };
    }
}
