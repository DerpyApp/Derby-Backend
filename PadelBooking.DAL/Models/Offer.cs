using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.DAL.Enums;
using PadelBooking.DAL.Models;

namespace PadelBooking.DAL.Entities;

public class Offer
{
    public int Id { get; set; }

    public int ClubId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DiscountType DiscountType { get; set; }

    public decimal DiscountValue { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public OfferStatus Status { get; set; }


    // Navigation Property

    public Club Club { get; set; } = null!;
}
