using System.Collections.Generic;
using System.Linq;
using LaDOSE.Business.Interface;
using LaDOSE.Entity.Context;
using LaDOSE.Entity.Wordpress;

namespace LaDOSE.Business.Service
{
    public class WordPressService : IWordPressService
    {
        private LaDOSEDbContext _context;
        public WordPressService(LaDOSEDbContext context)
        {
            this._context = context;
        }

        public List<WPEvent> GetWpEvent()
        {
            return _context.Set<WPEvent>().ToList();
        }
    }
}