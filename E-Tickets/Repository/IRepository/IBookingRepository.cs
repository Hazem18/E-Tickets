using E_Tickets.Models;

namespace E_Tickets.Repository.IRepository
{
    public interface IBookingRepository : IDbRepository<Booking>
    {
        Booking? GetBooking(int movieId, string userId);

        void DeleteBooking(Booking booking);

        IEnumerable<Booking> GetUserBookings(string userId);
    }
}
