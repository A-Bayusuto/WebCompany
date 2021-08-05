using System;
using System.Collections.Generic;
using System.Text;

namespace KnifeCompany.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Category { get; }
        ICompanyRepository Company { get; }
        IProductRepository Product { get; }
        IApplicationUserRepository ApplicationUser { get; }
        ISP_Call SP_Call { get; }

        void Save();
    }
}
