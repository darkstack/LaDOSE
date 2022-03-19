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

        public override Game AddOrUpdate(Game entity)
        {
            if (entity.Order == 0)
            {
                entity.Order = GetNextFreeOrder();
            }

            return base.AddOrUpdate(entity);
        }

        public override IEnumerable<Game> GetAll()
        {
            return _context.Game.ToList();
        }

        public int GetNextFreeOrder()
        {
            int nextFreeOrder = _context.Game.Max(e => e.Order);
            return ++nextFreeOrder;
        }
    }
}