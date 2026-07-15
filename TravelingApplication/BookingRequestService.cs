using Microsoft.EntityFrameworkCore;

namespace TravelingApplication
{
    public class BookingRequestService
    {
        private readonly AppDbContext _context;

        public BookingRequestService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddOrUpdateBookingRequest(BookingRequest bookingRequest)
        {
            var existingRequest = await _context.BookingRequests
                .FirstOrDefaultAsync(x => x.UserId == bookingRequest.UserId);

            if (existingRequest != null)
            {
                existingRequest.Link = bookingRequest.Link;
            }
            else
            {
                await _context.BookingRequests.AddAsync(bookingRequest);
            }

            await _context.SaveChangesAsync();
        }

        public List<BookingRequest> GetBookingRequest()
        {
            return _context.BookingRequests.ToList();
        }

        public async Task<BookingRequest> FindBookingRequest(int id)
        {
            return await _context.BookingRequests.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task UpdateBookingRequest(BookingRequest bookingRequest, string link)
        {
            if (bookingRequest != null)
            {
                bookingRequest.Link = link;
                await _context.SaveChangesAsync();
            }
        }
    }
}

