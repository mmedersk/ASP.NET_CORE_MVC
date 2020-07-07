using System;
using System.Collections.Generic;
using System.Text;
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;

namespace BulkyBook.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        public ICategoryRepository Category { get; private set; }
        public ISP_Call SpCall { get; private set; }

        public ISP_Call SP_Call => throw new NotImplementedException();

        public UnitOfWork(ApplicationDbContext context)
        {
            _dbContext = context;
            Category = new CategoryRepository(_dbContext);
            SpCall = new SP_Call(_dbContext);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
