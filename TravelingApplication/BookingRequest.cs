namespace TravelingApplication
{
    public class BookingRequest
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? Link { get; set; }
        public User? ApplicationUser { get; set; }
    }
}
