using E_Tickets.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Tickets.Controllers
{
    public class CinemaController : Controller
    {
        public ApplicationDbContext context { get; set; } = new ApplicationDbContext();
        public IActionResult Index()
        {
            var cinemas = context.Cinemas.ToList();
            return View(cinemas);
        }

        public IActionResult AllMovies(int Id)
        {
            var movies = context.Movies
             .Include(e => e.Cinema)
             .Include(e => e.Category)
             .Where(e => e.CinemaId == Id)
             .ToList();
            return View(movies);

        }

    }
}
