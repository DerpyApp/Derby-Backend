using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadelBooking.BLL.DTOs.AdminDTOs
{
   
        public class AdminAnalyticsDto
        {
            public int TotalUsers { get; set; }

            public int TotalClubs { get; set; }

            public int TotalBookings { get; set; }

            public int ConfirmedBookings { get; set; }

            public int CancelledBookings { get; set; }

            public decimal TotalRevenue { get; set; }
       
    
}
}
