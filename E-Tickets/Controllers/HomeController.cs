using E_Tickets.Data;
using E_Tickets.Models;
using E_Tickets.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace E_Tickets.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Repository.IRepository.IDbRepository<Movie> dbRepository;
        //public HomeController(IDbRepository<Movie> dbRepository)
        //{
        //    this.dbRepository=dbRepository;
        //}
        public HomeController(ILogger<HomeController> logger , IDbRepository<Movie> dbRepository)
        {
            _logger = logger;
             this.dbRepository=dbRepository;
        }

        public IActionResult Index()
        {
            var includeExpression = new List<Expression<Func<Movie, object>>>
            {
                c => c.Cinema,
                c => c.Category
            };
            var movies = dbRepository.GetAll(includeExpression);
            return View(movies);
        }
        public IActionResult Details(int Id)
        {
            var includeExpression = new List<Expression<Func<Movie, object>>>
            {
                c => c.Cinema,
                c => c.Category,
                c => c.Actors
            };
            var movie = dbRepository.GetAll(includeExpression).FirstOrDefault(e => e.Id == Id);
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
