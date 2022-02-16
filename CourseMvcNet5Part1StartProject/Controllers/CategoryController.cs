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
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            IEnumerable<Category> categories =_dbContext.Category.ToList();
            return View(categories);
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
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Category.Add(category);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        //GET : Edit
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _dbContext.Category.Find(id);
            if(obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        //POST : Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Category.Update(category);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        //Get : Delete
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _dbContext.Category.Find(id);
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
            var obj = _dbContext.Category.Find(id);

            if (obj == null)
            {
                return NotFound();
            }
            _dbContext.Category.Remove(obj);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
