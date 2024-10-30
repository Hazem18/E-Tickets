using E_Tickets.Models;
using E_Tickets.Repository;
using E_Tickets.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Linq.Expressions;

namespace E_Tickets.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingRepository bookingRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public BookingController(IBookingRepository bookingRepository, UserManager<ApplicationUser> userManager )
        {
            this.bookingRepository=bookingRepository;
            this.userManager=userManager;
        }
        public IActionResult Index()
        {
            var appUser = userManager.GetUserId(User);
            var includeExpression = new List<Expression<Func<Booking, object>>>
            {
                c => c.Movie,
                c=>c.ApplicationUser,
                c=>c.Movie.Cinema,
                c=>c.Movie.Category
            };
            var bookings = bookingRepository.GetAll(includeExpression).Where(e=>e.ApplicationUserId == appUser);

            ViewBag.Cost = bookings.Sum(e => e.NumberOfTickets * e.Movie.Price);

            return View(bookings.ToList());
        }

        public IActionResult AddBooking(int MovieId, int NumberOfTickets = 1)
        {
            var applicationUserId = userManager.GetUserId(User);

             if(applicationUserId == null )
            {
                return RedirectToAction("Login", "Account");
            }

            // Check if the booking already exists
            var existingBooking = bookingRepository.GetBooking(MovieId, applicationUserId ?? "");

            if (existingBooking != null)
            {
                
                existingBooking.NumberOfTickets += NumberOfTickets; 
                bookingRepository.Update(existingBooking); 
            }
            else
            {
                // Create a new booking if it doesn't exist
                Booking booking = new Booking()
                {
                    MovieId = MovieId,
                    NumberOfTickets = NumberOfTickets,
                    ApplicationUserId = applicationUserId
                };
                bookingRepository.Create(booking);
            }

            bookingRepository.Commit();
            TempData["success"] = "Tickets booked successfully....";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Increment(int MovieId)
        {
            var appUser = userManager.GetUserId(User);
            var booking = bookingRepository.GetBooking(MovieId, appUser ?? "");
            if (booking != null)
                booking.NumberOfTickets++;

            bookingRepository.Commit();
            return RedirectToAction("Index");
        }

        public IActionResult Decrement(int MovieId)
        {
            var appUser = userManager.GetUserId(User);
            var booking = bookingRepository.GetBooking(MovieId, appUser ?? "");
            if (booking != null)
            {
                booking.NumberOfTickets--;
                if(booking.NumberOfTickets == 0)
                    bookingRepository.DeleteBooking(booking);
            }
            bookingRepository.Commit();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int MovieId)
        {
            var appUser = userManager.GetUserId(User);
            var booking = bookingRepository.GetBooking(MovieId, appUser ?? "");
            if (booking != null)
            { 
              bookingRepository.DeleteBooking(booking);
            }
            bookingRepository.Commit();
            return RedirectToAction("Index");
        }

        public IActionResult Pay()
        {
            var appUser = userManager.GetUserId(User);
            //var cartDBs = cartRepository.Get(includeProps: [e => e.Product], expression: e => e.ApplicationUserId == appUser).ToList();
            var includeExpression = new List<Expression<Func<Booking, object>>>
            {
                c => c.Movie,
                c=>c.ApplicationUser,
                c=>c.Movie.Cinema,
                c=>c.Movie.Category
            };
            var bookings = bookingRepository.GetAll(includeExpression).Where(e => e.ApplicationUserId == appUser).ToList();
            
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/checkout/success",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/checkout/cancel",
            };

            foreach (var item in bookings)
            {
                var result = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Movie.Name,
                        },
                        UnitAmount = (long)item.Movie.Price * 100,
                    },
                    Quantity = item.NumberOfTickets,
                };
                options.LineItems.Add(result);

            }
          
            
            var service = new SessionService();
            var session = service.Create(options);
           
            return Redirect(session.Url);
        }

    }
}
