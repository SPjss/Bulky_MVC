using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRespository;
using BulkyBook.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class ProductsRepository : Repository<Product>, IProductsRepository 
    {
        private ApplicationDbContext _db;
        public ProductsRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }
     

        public void Update(Product obj)
        {
            var objFromDb =  _db.Products.FirstOrDefault(u=>u.Id == obj.Id);
            if (objFromDb != null) 
            {
                objFromDb.Tittle = obj.Tittle;

                objFromDb.ISBN = obj.ISBN;

                objFromDb.Price = obj.Price;

                objFromDb.Price50 = obj.Price50;

                objFromDb.Price100 = obj.Price100;

                objFromDb.ListPrice = obj.ListPrice;

                objFromDb.Description = obj.Description;

                objFromDb.CategoryId = obj.CategoryId;

                objFromDb.Author = obj.Author;
                if (obj.ImageUrl != null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }

            }
        }
    }
}
