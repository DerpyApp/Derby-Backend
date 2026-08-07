using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadelBooking.DAL.Models
{
    public class Wallet
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public decimal Balance { get; set; }

        public DateTime CreatedAt { get; set; }


        // Navigation Properties

        public User User { get; set; } = null!;

        public ICollection<WalletTransaction> Transactions { get; set; }
            = new HashSet<WalletTransaction>();
    }
}
