using E_Tickets.Data;
using E_Tickets.Models;
using E_Tickets.Repository;
using E_Tickets.Repository.IRepository;
using E_Tickets.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace E_Tickets.Controllers
{
    [Authorize(Roles = SD.adminRole)]
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
            Category category = new Category();
            return View(category);
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                CategorydbRepository.Create(category);
                CategorydbRepository.Commit();
                return RedirectToAction("Index");
            }
            return View(category);
        }
        public IActionResult Edit(int Id)
        {
            var category = CategorydbRepository.GetById(Id);
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                CategorydbRepository.Update(category);
                CategorydbRepository.Commit();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Delete(int Id)
        {
            CategorydbRepository.Delete(Id);
            CategorydbRepository.Commit();
            return RedirectToAction("Index");
        }
    }
}
