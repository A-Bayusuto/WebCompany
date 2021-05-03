using KnifeCompany.DataAccess.Data;
using KnifeCompany.DataAccess.Repository.IRepository;
using KnifeCompany.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KnifeCompany.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(Product product)
        {
            var objFromDb = _db.Products.FirstOrDefault(s => s.Id == product.Id);

            if (objFromDb != null) 
            {
                objFromDb.Name = product.Name;
                objFromDb.Price = product.Price;
                objFromDb.Status = product.Status;
                objFromDb.Description = product.Description;
                objFromDb.Picture = product.Picture;

            }


        }
    }
}
