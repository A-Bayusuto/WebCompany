using KnifeCompany.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KnifeCompany.DataAccess.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        void Update(ShoppingCart obj);
    }
}
