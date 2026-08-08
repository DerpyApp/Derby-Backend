using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadelBooking.DAL.Enums
{
    public enum PaymentStatus
    {
        Pending = 1,
        Paid = 2,
        PartiallyPaid = 3,
        Failed = 4,
        Refunded = 5,
        Cancelled = 6
    }
}
