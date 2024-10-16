using E_Tickets.Data;
using E_Tickets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace E_Tickets.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public ApplicationDbContext context { get; set; } = new ApplicationDbContext();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var movies = context.Movies.
                Include(e =>  e.Category)
                .Include(e => e.Cinema)
                .Select(e => new {e.Id, e.Name, e.Price, e.Category, e.StartDate, e.EndDate, e.Cinema , e.MovieStatus, e.ImgUrl })
                .ToList();
            return View(movies);
        }
        public IActionResult Details(int Id)
        {

            var movie = context.Movies.
                Include(e => e.Category)
                .Include(e => e.Cinema)
                .Include (e => e.Actors)
                .Select(e => new {e.Id , e.Name, e.Price, e.Category, e.StartDate, e.EndDate, e.Cinema, e.MovieStatus, e.ImgUrl , e.Actors })
                .FirstOrDefault(e => e.Id == Id);
            return View(movie);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
