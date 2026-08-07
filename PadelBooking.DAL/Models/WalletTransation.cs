using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.DAL.Models;

namespace PadelBooking.DAL.Entities;

public class WalletTransaction
{
    public int Id { get; set; }

    public int WalletId { get; set; }

    public decimal Amount { get; set; }

    public WalletTransactionType Type { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }


    // Navigation Property

    public Wallet Wallet { get; set; } = null!;
}  }
}
