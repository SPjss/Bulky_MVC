using BulkyBook.DataAccess.Repository.IRespository;
using BulkyBook.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using BulkyBook.Models.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using BulkyBook.Models.ViewModals;
using System.IO;
using System.IO.Pipes;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IUnitofWork _unitofwork;
        private readonly IWebHostEnvironment _webHostEnvironment;   
        public ProductsController(IUnitofWork unitofwork, IWebHostEnvironment webHostEnvironment)
        {
            _unitofwork = unitofwork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductsList = _unitofwork.Products.GetAll(includeProperties:"Category").ToList();

            return View(objProductsList);
        }
        public IActionResult Upsert(int? id)
        {

            ProductVM productVM = new()
            {
                CategoryList = _unitofwork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.CategoryId.ToString()
                }),
                Product = new Product()
             };
            if(id == null || id == 0)
            {
                // to create
                return View(productVM);

            }
            else
            {
                //to update
                productVM.Product = _unitofwork.Products.Get(u=>u.Id==id);
                return View(productVM);
            }

        }

          [HttpPost]
          public IActionResult Upsert(ProductVM productVM , IFormFile? file)
          {
              if (ModelState.IsValid)
              {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null) 
                {
                    string fileName = Guid.NewGuid().ToString() + Path .GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"Images\Product");
                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl)) 
                    {
                        //delete old image
                       var oldImagepath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagepath))
                        {
                            System.IO.File.Delete(oldImagepath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                   
                    productVM.Product.ImageUrl = @"\Images\Product\" + fileName;
                }
                if(productVM.Product.Id == 0)
                {
                    _unitofwork.Products.Add(productVM.Product);
                }
                else
                {
                    _unitofwork.Products.Update(productVM.Product);

                }
                _unitofwork.Save();
                TempData["Success"] = "Products Created Successfully";
                return RedirectToAction("Index");
              }
            else
            {
                productVM.CategoryList = _unitofwork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.CategoryId.ToString()
                });
               return View(productVM);
            }
          }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll() 
        {
            List<Product> objProductsList = _unitofwork.Products.GetAll(includeProperties: "Category").ToList();
            return Json(new {data = objProductsList});
        }
      
        public IActionResult Delete(int? id)
        {
            var producttobeDeleted = _unitofwork.Products.Get(u=>u.Id == id);
            if (producttobeDeleted == null)
            {
                return Json(new {success = false, message = "Error while deleting" });
            }

            var oldImagepath = Path.Combine(_webHostEnvironment.WebRootPath, producttobeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagepath))
            {
                System.IO.File.Delete(oldImagepath);
            }
            _unitofwork.Products.Remove(producttobeDeleted);
            _unitofwork.Save();
            List<Product> objProductsList = _unitofwork.Products.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductsList });
        }

        #endregion
    }
}
