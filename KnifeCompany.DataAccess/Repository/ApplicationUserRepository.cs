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


    }
}
