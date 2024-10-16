using E_Tickets.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Tickets.Controllers
{
    public class CategoryController : Controller
    {
        public ApplicationDbContext context { get; set; } = new ApplicationDbContext();
        public IActionResult Index()
        {
            var categoies = context.Categories.ToList();

            return View(categoies);
        }
        public IActionResult AllMovies(int Id)
        {
            var movies = context.Movies
             .Include(e => e.Cinema)
             .Include(e => e.Category)
             .Where(e => e.CategoryId == Id)
             .ToList();
            return View(movies);

        }
    }
}
