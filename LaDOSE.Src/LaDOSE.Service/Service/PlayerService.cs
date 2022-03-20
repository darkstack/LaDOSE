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

        public int GetBySmash(ParticipantType participantUser)
        {
            //var p2 = _context.Player.ToList();

            var p = _context.Player.FirstOrDefault(e => e.SmashId == participantUser.user.id);
            if (p == null)
            {
                var entity = new Player()
                {
                    Gamertag = participantUser.gamerTag,
                    Name = string.IsNullOrEmpty(participantUser.user.name)? participantUser.gamerTag : participantUser.user.name,
                    SmashId = participantUser.user.id,
                };
                _context.Player.Add(entity);
                _context.SaveChanges();
                return entity.Id;
            }

            if (p.Gamertag != participantUser.gamerTag)
            {
                p.Name = participantUser.gamerTag;
                _context.SaveChanges();
            }

            return p.Id;

        }
    }
}