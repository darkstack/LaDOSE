using System;
using System.Collections.Generic;
using System.Linq;
using LaDOSE.Business.Interface;
using LaDOSE.Entity;
using LaDOSE.Entity.Context;
using Microsoft.EntityFrameworkCore;

namespace LaDOSE.Business.Service
{
    public class GameService : BaseService<Game> ,IGameService
    {

        public GameService(LaDOSEDbContext context) : base(context)
        {
        }

        public override IEnumerable<Game> GetAll()
        {
            return _context.Game.Include(e => e.Seasons).ToList();
        }


    }
}