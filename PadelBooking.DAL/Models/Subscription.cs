using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.DAL.Models;

namespace PadelBooking.DAL.Entities;

public class Subscription
{
    public int Id { get; set; }

    public int ClubId { get; set; }

    public SubscriptionPlan Plan { get; set; }

    public decimal Price { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public SubscriptionStatus Status { get; set; }


    // Navigation Property

    public Club Club { get; set; } = null!;
}
