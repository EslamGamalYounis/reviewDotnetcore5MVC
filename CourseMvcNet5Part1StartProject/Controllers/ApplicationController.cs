using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseMvcNet5Part1StartProject.Data;
using CourseMvcNet5Part1StartProject.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CourseMvcNet5Part1StartProject.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public ApplicationController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            IEnumerable<Application> applications = _dbContext.Application.ToList();
            if (applications != null)
                return View(applications);

            return View();
        }

        //GET : Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //POST : Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Application application)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Application.Add(application);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(application);
        }

        //GET : Edit
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _dbContext.Application.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        //POST : Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Application app)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Application.Update(app);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(app);
        }

        //Get : Delete
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _dbContext.Application.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        //POST : Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _dbContext.Application.Find(id);

            if (obj == null)
            {
                return NotFound();
            }
            _dbContext.Application.Remove(obj);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
