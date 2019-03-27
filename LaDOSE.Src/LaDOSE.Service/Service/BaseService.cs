using System;
using System.Collections.Generic;
using System.Linq;
using LaDOSE.Business.Interface;
using LaDOSE.Entity.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LaDOSE.Business.Service
{
    public class BaseService<T> : IBaseService<T> where T : Entity.Context.Entity
    {
        protected LaDOSEDbContext _context;

        public BaseService(LaDOSEDbContext context)
        {
            _context = context;
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
            try
            {

                var entityEntry = _context.Update(entity);
                this._context.SaveChanges();
                return true;
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public virtual bool Delete(int id)
        {
            try
            {
                var find = _context.Find<T>(id);
                if (find == null) return false;
                _context.Remove((object) find);
                this._context.SaveChanges();
                return true;
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            
        }

        public virtual T AddOrUpdate(T entity)
        {
            EntityEntry<T> entityEntry;
            if (entity.Id == 0)
            {
                entityEntry = this._context.Add(entity);
            }
            else
            {
                entityEntry  = this._context.Update(entity);
            }

            this._context.SaveChanges();
            return entityEntry.Entity;
        }
    }
}