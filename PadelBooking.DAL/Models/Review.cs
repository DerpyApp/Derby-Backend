using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadelBooking.DAL.Models
{
    public class Review
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ClubId { get; set; }

        public int Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation properties

        public User User { get; set; } = null!;
        public Club Club { get; set; } = null!;
    }
}
