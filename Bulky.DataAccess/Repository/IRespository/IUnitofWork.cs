using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRespository
{
    public interface IUnitofWork
    {
        ICategoryRepository Category{ get; }
        IProductsRepository Products { get; }
        public void Save();
    }
}
