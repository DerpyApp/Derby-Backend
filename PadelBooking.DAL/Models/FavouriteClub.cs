using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadelBooking.DAL.Models
{
    public class FavouriteClub
    {
        public int UserId { get; set; }

        public int ClubId { get; set; }

        public DateTime CreatedAt { get; set; }


        // Navigation Properties

        public User User { get; set; } = null!;

        public Club Club { get; set; } = null!;
    }
}
