using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CourseMvcNet5Part1StartProject.Data;
using CourseMvcNet5Part1StartProject.Models;
using CourseMvcNet5Part1StartProject.Models.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CourseMvcNet5Part1StartProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductController(ApplicationDbContext dbContext,IWebHostEnvironment _webHostEnvironment)
        {
            _dbContext = dbContext;
            webHostEnvironment = _webHostEnvironment;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            IEnumerable<Product> products =_dbContext.Product.Include(i=>i.Category).Include(i=>i.ApplicationType).ToList();

            //foreach (var obj in products)
            //{
            //    obj.Category = _dbContext.Category.FirstOrDefault(u => u.Id == obj.CategoryId);
            //    obj.ApplicationType = _dbContext.Application.FirstOrDefault(u => u.Id == obj.ApplicationId);
            //}
            return View(products);
        }

        //GET : Upsert
        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> CategoryDropDown = _dbContext.Category.Select(i => new SelectListItem
            //{
            //    Text = i.Name,
            //    Value = i.Id.ToString()
            //});

            //ViewBag.CategoryDropDown = CategoryDropDown;
            //ViewData["CategoryDropDown"] = CategoryDropDown;
            //Product product = new Product();

            //using viewmodel
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategorySelectList = _dbContext.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                ApplicationSelectList = _dbContext.Application.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                //this is for create
                return View(productVM);
            }
            else
            {
                productVM.Product = _dbContext.Product.Find(id);
                if (productVM.Product == null)
                {
                    return NotFound();
                }
                return View(productVM);
            }    
        }

        //POST : Upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                //_dbContext.Product.Add(product);
                //_dbContext.SaveChanges();

                var files = HttpContext.Request.Form.Files;
                string webRootPath = webHostEnvironment.WebRootPath;

                if(productVM.Product.Id == 0)
                {
                    //creating
                    string upload = webRootPath + WebConstant.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension),FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVM.Product.Image = fileName + extension;
                    _dbContext.Add(productVM.Product);  
                }
                else
                {
                    //updating
                    var objFromDb = _dbContext.Product.AsNoTracking().FirstOrDefault(i => i.Id == productVM.Product.Id );
                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WebConstant.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload,objFromDb.Image);
                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productVM.Product.Image = fileName + extension;
                    }
                    else
                    {
                        productVM.Product.Image = objFromDb.Image;
                    }
                    _dbContext.Product.Update(productVM.Product);

                }

                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            productVM.CategorySelectList = _dbContext.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            productVM.ApplicationSelectList = _dbContext.Application.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View(productVM);
        }

        

        //Get : Delete
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product product = _dbContext.Product.Include(u=>u.Category).Include(u=>u.ApplicationType).FirstOrDefault(u=>u.Id == id);
            //product.Category = _dbContext.Category.Find(product.CategoryId);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //POST : Delete
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _dbContext.Product.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            if (obj.Image != null)
            {
                string upload = webHostEnvironment.WebRootPath + WebConstant.ImagePath;
                var oldFile = Path.Combine(upload, obj.Image);
                if (System.IO.File.Exists(oldFile))
                {
                    System.IO.File.Delete(oldFile);
                }
            }

            _dbContext.Product.Remove(obj);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
