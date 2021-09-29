using KnifeCompany.DataAccess.Data;
using KnifeCompany.DataAccess.Repository.IRepository;
using KnifeCompany.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KnifeCompany.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ApplicationUser user)
        {
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(s => s.Id == user.Id);

            if (objFromDb != null)
            {

                objFromDb.Name = user.Name;
                objFromDb.StreetAddress = user.StreetAddress;
                objFromDb.City = user.City;
                objFromDb.State = user.State;
                objFromDb.PostalCode = user.PostalCode;
                objFromDb.PhoneNumber = user.PhoneNumber;

            }


        }

    }
}
