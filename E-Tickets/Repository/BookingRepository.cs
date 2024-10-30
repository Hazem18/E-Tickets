using E_Tickets.Data;
using E_Tickets.Models;
using E_Tickets.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace E_Tickets.Repository
{
    public class BookingRepository : DbRepository<Booking> , IBookingRepository
    {
        private readonly ApplicationDbContext dbContext;

        public BookingRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext=dbContext;
        }

        public Booking? GetBooking(int movieId, string userId)
        {
            return dbContext.Bookings
                .FirstOrDefault(b => b.MovieId == movieId && b.ApplicationUserId == userId);
        }
      
        public void DeleteBooking(Booking booking)
        {
            dbContext.Bookings.Remove(booking);
        }

        public IEnumerable<Booking> GetUserBookings(string userId)
        {
            return dbContext.Bookings.Where(b => b.ApplicationUserId == userId).ToList();
        }
    }
}
