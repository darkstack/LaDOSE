using System;
using System.Collections.Generic;
using System.Linq;
using LaDOSE.Business.Interface;
using LaDOSE.Entity;
using LaDOSE.Entity.Context;
using Microsoft.EntityFrameworkCore;

namespace LaDOSE.Business.Service
{
    public class TodoService : BaseService<Todo>, ITodoService
    {
        public TodoService(LaDOSEDbContext context) : base(context)
        {
        }

        public override bool Delete(int id)
        {
            var find = _context.Find<Todo>(id);
            find.Deleted = DateTime.Now;
            this._context.SaveChanges();
            return _context.Entry(find).State == EntityState.Modified;
        }

        public override IEnumerable<Todo> GetAll()
        {
            return _context.Set<Todo>().Where(e=>e.Deleted == null).ToList();
        }
    }
}