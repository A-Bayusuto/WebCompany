using KnifeCompany.DataAccess.Data;
using KnifeCompany.DataAccess.Repository.IRepository;
using KnifeCompany.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KnifeCompany.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(OrderHeader obj)
        {
            //var objFromDb = _db.OrderHeaders.FirstOrDefault(s => s.ApplicationId == obj.ApplicationId);

            //if (objFromDb != null)
            //{
            //    objFromDb.ApplicationId = obj.OrderStatus;
            //    objFromDb.OrderDate = obj.OrderDate;
            //    objFromDb.OrderTotal = obj.OrderTotal;
            //    objFromDb.OrderStatus = obj.OrderStatus;
            //    objFromDb.PaymentStatus = obj.PaymentStatus;

            //}
            _db.Update(obj);
        }
    }
}
