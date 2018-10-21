using LaDOSE.Business.Interface;
using LaDOSE.Entity.Context;
using Microsoft.EntityFrameworkCore;

namespace LaDOSE.Business.Service
{
    public class UtilService : IUtilService
    {
        private LaDOSEDbContext _context;

        public UtilService(LaDOSEDbContext context)
        {
            _context = context;
        }

        public bool UpdateBooking()
        {
            _context.Database.SetCommandTimeout(60);
            _context.Database.ExecuteSqlCommand("call ladoseapi.ImportEvent();");
            _context.Database.SetCommandTimeout(30);
            return true;
        }
    }
}