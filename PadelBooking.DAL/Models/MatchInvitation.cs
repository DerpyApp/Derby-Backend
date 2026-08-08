using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.DAL.Enums;

namespace PadelBooking.DAL.Models
{
    public class MatchInvitation
    {
        public int Id { get; set; }

        public int BookingId { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        public InvitationStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }


        // Navigation Properties

        public Booking Booking { get; set; } = null!;

        public User Sender { get; set; } = null!;

        public User Receiver { get; set; } = null!;
    }
}
