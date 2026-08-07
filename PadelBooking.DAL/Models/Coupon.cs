using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PadelBooking.DAL.Enums;

namespace PadelBooking.DAL.Entities;

public class Coupon
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public DiscountType DiscountType { get; set; }

    public decimal DiscountValue { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int? UsageLimit { get; set; }

    public CouponStatus Status { get; set; }


    // Navigation Property

    public ICollection<BookingCoupon> BookingCoupons { get; set; }
        = new HashSet<BookingCoupon>();
}
