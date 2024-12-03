using BulkyBook.DataAccess.Repository;
using BulkyBook.DataAccess.Repository.IRespository;
using BulkyBook.Models;
using BulkyBook.Models.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitofWork _unitofwork;

        public HomeController(ILogger<HomeController> logger, IUnitofWork unitofWork)
        {
            _logger = logger;
            _unitofwork = unitofWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product>productlist = _unitofwork.Products.GetAll(includeProperties:"Category");
            return View(productlist);
        }
        public IActionResult Details(int productId)
        {
            Product product = _unitofwork.Products.Get(u=>u.Id==productId , includeProperties: "Category");
            return View(product);
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
