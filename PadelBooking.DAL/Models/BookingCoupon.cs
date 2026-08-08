using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.DAL.Models;

namespace PadelBooking.DAL.Entities;

public class BookingCoupon
{
    public int BookingId { get; set; }

    public int CouponId { get; set; }

    public decimal DiscountAmount { get; set; }

    public DateTime AppliedAt { get; set; }


    // Navigation Properties

    public Booking Booking { get; set; } = null!;

    public Coupon Coupon { get; set; } = null!;
}
