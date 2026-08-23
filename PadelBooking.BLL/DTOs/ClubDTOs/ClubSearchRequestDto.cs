using System;

namespace PadelBooking.BLL.DTOs.ClubDTOs
{
    public class ClubSearchRequestDto
    {
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }

        public decimal Latitude
        {
            get => Lat != 0 ? Lat : _latitude;
            set => _latitude = value;
        }
        private decimal _latitude;

        public decimal Longitude
        {
            get => Lng != 0 ? Lng : _longitude;
            set => _longitude = value;
        }
        private decimal _longitude;

        public double Radius { get; set; } = 10;
    }
}
