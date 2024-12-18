﻿using E_Tickets.Data;
using E_Tickets.Models;
using E_Tickets.Repository.IRepository;
using E_Tickets.Utility;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace E_Tickets.Controllers
{
    
    public class CinemaController : Controller
    {
        private readonly ICinemaRepository CinemadbRepository;
        private readonly IMovieRepository MoviedbRepository;

        

        public CinemaController(ICinemaRepository CinemadbRepository , IMovieRepository MoviedbRepository)
        {
            this.CinemadbRepository=CinemadbRepository;
            this.MoviedbRepository=MoviedbRepository;
        }
        [Authorize(Roles = SD.adminRole)]
        public IActionResult Index(int page =1 , int pagesize = 3)
        {

            var cinemas = CinemadbRepository.GetAll().AsQueryable();

            var totalCinemas = cinemas.Count();
            
            var cinemasList = cinemas
                .Skip((page - 1) *pagesize)
                .Take(pagesize)
                .ToList();

            ViewBag.TotalCinemas = totalCinemas;
            ViewBag.Page = page;
            ViewBag.PageSize = pagesize;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCinemas / pagesize);

            return View(cinemasList);
        }

        [Authorize(Roles = $"{SD.adminRole},{SD.UserRole}")]
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
        [Authorize(Roles = SD.adminRole)]
        public IActionResult Create()
        {
            Cinema cinema = new Cinema();
            return View(cinema);
        }
        [Authorize(Roles = SD.adminRole)]
        public IActionResult Edit(int Id)
        {
            var cinema = CinemadbRepository.GetById(Id);
            return View(cinema);
        }

        [HttpPost]
        [Authorize(Roles = SD.adminRole)]
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
        [Authorize(Roles = SD.adminRole)]
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
        [Authorize(Roles = SD.adminRole)]
        public IActionResult Delete(int Id)
        {
            CinemadbRepository.Delete(Id);
            CinemadbRepository.Commit();
            return RedirectToAction("Index");
        }

    }
}
