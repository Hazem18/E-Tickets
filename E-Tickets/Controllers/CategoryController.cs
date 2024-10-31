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
  
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository CategorydbRepository;
        private readonly IMovieRepository MoviedbRepository;

        public CategoryController(ICategoryRepository CategorydbRepository , IMovieRepository MoviedbRepository )
        {
            this.CategorydbRepository=CategorydbRepository;
            this.MoviedbRepository=MoviedbRepository;
        }
        [Authorize(Roles = SD.adminRole)]
        public IActionResult Index(int page = 1, int pageSize = 3)
        {
            var categories = CategorydbRepository.GetAll().AsQueryable();

            var totalCategories = categories.Count();


            var categoriesList = categories
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

 
            ViewBag.TotalCategories = totalCategories;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;

            // Calculate total pages
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCategories / pageSize);

            return View(categoriesList);
        }
        [Authorize(Roles = $"{SD.adminRole},{SD.UserRole}")]
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
        [Authorize(Roles = SD.adminRole)]
        public IActionResult Create()
        {
            Category category = new Category();
            return View(category);
        }

        [HttpPost]
        [Authorize(Roles = SD.adminRole)]
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
        [Authorize(Roles = SD.adminRole)]
        public IActionResult Edit(int Id)
        {
            var category = CategorydbRepository.GetById(Id);
            return View(category);
        }

        [HttpPost]
        [Authorize(Roles = SD.adminRole)]
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
        [Authorize(Roles = SD.adminRole)]
        public IActionResult Delete(int Id)
        {
            CategorydbRepository.Delete(Id);
            CategorydbRepository.Commit();
            return RedirectToAction("Index");
        }
    }
}
