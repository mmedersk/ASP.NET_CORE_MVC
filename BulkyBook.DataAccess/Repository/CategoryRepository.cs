using System;
using System.Collections.Generic;
using System.Text;
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository
{
    class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public void Update(Category c)
        {
            var catInDb = _dbContext.Categories.Find(c.Id);
            if (catInDb != null)
            {
                catInDb.Name = c.Name;
                _dbContext.SaveChanges();
            }
        }
    }
}
