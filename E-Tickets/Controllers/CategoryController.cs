using E_Tickets.Data;
using E_Tickets.Models;
using E_Tickets.Repository;
using E_Tickets.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace E_Tickets.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IDbRepository<Category> CategorydbRepository;
        private readonly IDbRepository<Movie> MoviedbRepository;

       public CategoryController(IDbRepository<Category> CategorydbRepository , IDbRepository<Movie> MoviedbRepository)
        {
            this.CategorydbRepository=CategorydbRepository;
            this.MoviedbRepository=MoviedbRepository;
        }
        public IActionResult Index()
        {
            var categoies = CategorydbRepository.GetAll();

            return View(categoies);
        }
        public IActionResult AllMovies(int Id)
        {

            var includeExpression = new List<Expression<Func<Movie, object>>>
            {
                c => c.Cinema,
                c => c.Category
            };

            var movies = MoviedbRepository.GetAll(includeExpression)
                .Where(e=> e.CategoryId == Id);
            return View(movies);

        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            CategorydbRepository.Create(category);
            CategorydbRepository.Commit();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int Id)
        {
            var category = CategorydbRepository.GetById(Id);
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            CategorydbRepository.Update(category);
            CategorydbRepository.Commit();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int Id)
        {
            CategorydbRepository.Delete(Id);
            CategorydbRepository.Commit();
            return RedirectToAction("Index");
        }
    }
}
