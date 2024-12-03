using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRespository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class UnitofWork : IUnitofWork
    {
        private ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }
        public IProductsRepository Products { get; private set; }



        public UnitofWork(ApplicationDbContext db)  
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Products = new ProductsRepository(_db);

        }
        public void Save()
        {
            _db.SaveChanges();  
        }
    }
}
