using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CourseMvcNet5Part1StartProject.Models;
using CourseMvcNet5Part1StartProject.Data;
using CourseMvcNet5Part1StartProject.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using CourseMvcNet5Part1StartProject.Utility;

namespace CourseMvcNet5Part1StartProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext dbContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext _dbContext)
        {
            _logger = logger;
            dbContext = _dbContext;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Prodcuts = dbContext.Product.Include(i=>i.Category).Include(i => i.ApplicationType),
                Categories = dbContext.Category
            };
            return View(homeVM);
        }

        public IActionResult Details(int id)
        {
            List<ShoppingCart> shoppingCartsList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstant.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstant.SessionCart).Count() > 0)
            {
                shoppingCartsList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstant.SessionCart);
            }

            DetailsVM detailsVM = new DetailsVM
            {
                Product = dbContext.Product.Include(i => i.Category).Include(i => i.ApplicationType)
                .Where(i => i.Id == id).FirstOrDefault(),
                ExistsIncart = false
            };

            foreach (var item in shoppingCartsList)
            {
                if (item.ProductId == id)
                {
                    detailsVM.ExistsIncart = true;
                }
            }
            return View(detailsVM);
        }

        [HttpPost,ActionName("Details")]
        public IActionResult DetailsPost(int id)
        {
            List<ShoppingCart> shoppingCartsList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstant.SessionCart) !=null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstant.SessionCart).Count() > 0 )
            {
                shoppingCartsList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstant.SessionCart);
            }
            shoppingCartsList.Add(new ShoppingCart { ProductId = id });
            HttpContext.Session.Set(WebConstant.SessionCart, shoppingCartsList);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(int id)
        {
            List<ShoppingCart> shoppingCartsList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstant.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstant.SessionCart).Count() > 0)
            {
                shoppingCartsList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstant.SessionCart);
            }

            var itemToRemove = shoppingCartsList.SingleOrDefault(i => i.ProductId == id);

            if (itemToRemove != null)
            {
                shoppingCartsList.Remove(itemToRemove);
            }
            HttpContext.Session.Set(WebConstant.SessionCart, shoppingCartsList);
            return RedirectToAction(nameof(Index));
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
