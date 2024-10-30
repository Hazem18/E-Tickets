using E_Tickets.Models;
using E_Tickets.Repository;
using E_Tickets.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Tickets.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IBookingRepository bookingRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public CheckoutController(IBookingRepository bookingRepository, UserManager<ApplicationUser> userManager)
        {
            this.bookingRepository=bookingRepository;
            this.userManager=userManager;
        }
        public IActionResult success()
        {
            var appUser = userManager.GetUserId(User);
            var userBookings = bookingRepository.GetAll().Where(b => b.ApplicationUserId == appUser).ToList();

            foreach (var booking in userBookings)
            {
                bookingRepository.DeleteBooking(booking);
            }

            bookingRepository.Commit();
            return View();
        }

        public IActionResult cancel()
        {
            return View();
        }
    }
}
