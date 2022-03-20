using System.Linq;
using LaDOSE.Business.Interface;
using LaDOSE.Business.Provider.SmashProvider;
using LaDOSE.Entity;
using LaDOSE.Entity.Context;
using Microsoft.EntityFrameworkCore.Internal;

namespace LaDOSE.Business.Service
{
    public class PlayerService : BaseService<Player>, IPlayerService
    {

        public PlayerService(LaDOSEDbContext context) : base(context)
        {
        }

        public int GetBySmash(PlayerType playerUser)
        {
            //var p2 = _context.Player.ToList();

            var p = _context.Player.FirstOrDefault(e => e.SmashId == playerUser.user.id);
            if (p == null)
            {
                var entity = new Player()
                {
                    Gamertag = playerUser.gamerTag,
                    Name = string.IsNullOrEmpty(playerUser.user.name)? playerUser.gamerTag : playerUser.user.name,
                    SmashId = playerUser.user.id,
                };
                _context.Player.Add(entity);
                _context.SaveChanges();
                return entity.Id;
            }

            if (p.Gamertag != playerUser.gamerTag)
            {
                p.Name = playerUser.gamerTag;
                _context.SaveChanges();
            }

            return p.Id;

        }
    }
}