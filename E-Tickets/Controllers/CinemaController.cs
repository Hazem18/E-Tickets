using E_Tickets.Data;
using E_Tickets.Models;
using E_Tickets.Repository.IRepository;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace E_Tickets.Controllers
{
    public class CinemaController : Controller
    {
        private readonly IDbRepository<Cinema> CinemadbRepository;
        private readonly IDbRepository<Movie> MoviedbRepository;

        

        public CinemaController(IDbRepository<Cinema> CinemadbRepository , IDbRepository<Movie> MoviedbRepository)
        {
            this.CinemadbRepository=CinemadbRepository;
            this.MoviedbRepository=MoviedbRepository;
        }
        public IActionResult Index()
        {
            var cinemas = CinemadbRepository.GetAll();
            return View(cinemas);
        }

        public IActionResult AllMovies(int Id)
        {
            var includeExpression = new List<Expression<Func<Movie , object>>>
            {
                c => c.Category,
                c => c.Cinema
            };
            var movies = MoviedbRepository.GetAll(includeExpression)
             .Where(e => e.CinemaId == Id);
             
            return View(movies);

        }
        public IActionResult Create()
        {
            Cinema cinema = new Cinema();
            return View(cinema);
        }
        public IActionResult Edit(int Id)
        {
            var cinema = CinemadbRepository.GetById(Id);
            return View(cinema);
        }

        [HttpPost]
        public IActionResult Create(Cinema cinema)
        {
            if (ModelState.IsValid)
            {
                CinemadbRepository.Create(cinema);
                CinemadbRepository.Commit();
                return RedirectToAction("Index");
            }
            return View(cinema);
        }

        [HttpPost]
        public IActionResult Edit(Cinema cinema)
        {
            if (ModelState.IsValid)
            {
                CinemadbRepository.Update(cinema);
                CinemadbRepository.Commit();
                return RedirectToAction("Index");
            }
            return View(cinema);
        }
        public IActionResult Delete(int Id)
        {
            CinemadbRepository.Delete(Id);
            CinemadbRepository.Commit();
            return RedirectToAction("Index");
        }

    }
}
