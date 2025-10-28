using Microsoft.AspNetCore.Mvc;

namespace HotelBookingService
{
    public class BookingDetails
    {
        public string CountryOrCity { get; set; }
        public DateTime Checkin { get; set; }
        public DateTime Checkout { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
    }
}
