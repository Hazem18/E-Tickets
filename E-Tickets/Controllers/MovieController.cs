using E_Tickets.Models;
using E_Tickets.Repository;
using E_Tickets.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace E_Tickets.Controllers
{
    public class MovieController : Controller
    {
        private readonly IDbRepository<Movie> MoviedbRepository;
        private readonly IDbRepository<Category> CategorydbRepository;
        private readonly IDbRepository<Cinema> CinemadbRepository;

        public MovieController(IDbRepository<Movie> MoviedbRepository, IDbRepository<Category> CategorydbRepository , IDbRepository<Cinema> CinemadbRepository)
        {
            this.MoviedbRepository=MoviedbRepository;
            this.CategorydbRepository=CategorydbRepository;
            this.CinemadbRepository=CinemadbRepository;
        }
        public IActionResult Index()
        {
            var includeExpression = new List<Expression<Func<Movie, object>>>
            {
                c => c.Category,
                c=>c.Cinema
            };
            var movies = MoviedbRepository.GetAll(includeExpression); 
            return View(movies);
        }

        public IActionResult Details(int Id)
        {
            var includeExpression = new List<Expression<Func<Movie, object>>>
            {
                c => c.Category,
                c=>c.Cinema,
                c=>c.Actors
            };
            var movie = MoviedbRepository.GetAll(includeExpression).FirstOrDefault(e => e.Id == Id);
            return View(movie);
        }
        public IActionResult Create()
        {
            LoadBags();
            return View();
        }
        public IActionResult Edit(int Id)
        {
            var movie = MoviedbRepository.GetAll().FirstOrDefault(e => e.Id == Id);
            LoadBags();
            return View(movie);
        }


        [HttpPost]
        public IActionResult Create(IFormFile ImgUrl, Movie movie)
        {
            movie.MovieStatus = DetermineMovieStatus(movie.StartDate, movie.EndDate);

            var fileName = UploadImg(ImgUrl);
            movie.ImgUrl = fileName ?? string.Empty;


            MoviedbRepository.Create(movie);
            MoviedbRepository.Commit();
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(IFormFile ImgUrl, Movie movie)
        {
            movie.MovieStatus = DetermineMovieStatus(movie.StartDate, movie.EndDate);

            movie.ImgUrl = UpdateMovieImage(ImgUrl, movie.Id);


            MoviedbRepository.Update(movie);
            MoviedbRepository.Commit();

            return RedirectToAction("Index");
        }
        public IActionResult Delete(int Id)
        {
            MoviedbRepository.Delete(Id);
            MoviedbRepository.Commit();

            return RedirectToAction("Index");
        }
        private void LoadBags()
        {
            ViewBag.Categories = CategorydbRepository.GetAll();
            ViewBag.Cinemas = CinemadbRepository.GetAll();
        }
        private MovieStatus DetermineMovieStatus(DateTime startDate, DateTime endDate)
        {
            if (endDate.Year > DateTime.Now.Year)
                return MovieStatus.Upcoming;
            if (endDate < DateTime.Now)
                return MovieStatus.Expired;
            if (startDate <= DateTime.Now && endDate >= DateTime.Now)
                return MovieStatus.Available;

            return MovieStatus.Upcoming; //movies with a future start date
        }

        private string? UploadImg(IFormFile ImgUrl)
        {
            // save in wwwroot
            if (ImgUrl.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImgUrl.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\movies", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    ImgUrl.CopyTo(stream);
                }
                return fileName;
            }
            return null;
        }

        private string UpdateMovieImage(IFormFile ImgUrl, int movieId)
        {
            // Check if a new image is uploaded
            if (ImgUrl != null && ImgUrl.Length > 0)
            {
                return UploadImg(ImgUrl) ?? string.Empty;
            }
            else
            {
                
                var existingMovie = MoviedbRepository.GetById(movieId);
                return existingMovie.ImgUrl;
            }
        }


    }
}
