using System.Collections.Generic;
using System.Linq;
using LaDOSE.Business.Interface;
using LaDOSE.Entity.Context;
using Microsoft.EntityFrameworkCore;

namespace LaDOSE.Business.Service
{


    public class BaseService<T> : IBaseService<T> where T : class 
    {
        protected LaDOSEDbContext _context;
        public BaseService(LaDOSEDbContext context)
        {
            this._context = context;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
        public virtual T GetById(int id)
        {
            return _context.Find<T>(id);
        }
        public virtual T Create(T entity)
        {
            var added = _context.Add(entity);
            return added.Entity;
        }
        public virtual bool Update(T entity)
        {
            var entityEntry = _context.Update(entity);
            return _context.Entry(entityEntry).State == EntityState.Unchanged;
        }
        public virtual bool Delete(int id)
        {
            var find = _context.Find<T>(id);
            _context.Remove(find);
            return _context.Entry(find).State == EntityState.Deleted;
        }
    }
}