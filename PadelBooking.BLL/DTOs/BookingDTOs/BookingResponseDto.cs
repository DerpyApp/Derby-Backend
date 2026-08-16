using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadelBooking.BLL.DTOs.BookingDTOs
{
    public class BookingResponseDto
    {
        public int BookingId { get; set; }

        public string Status { get; set; } = null!;

        public decimal DepositAmount { get; set; }

        public decimal Remaining { get; set; }

        public string? PaymentUrl { get; set; }
    }
}
