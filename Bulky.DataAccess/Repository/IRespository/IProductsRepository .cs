using BulkyBook.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRespository
{
    public interface IProductsRepository : IRepository<Product>
    {
        void Update(Product obj);
    }
}
