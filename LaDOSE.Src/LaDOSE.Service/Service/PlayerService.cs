using System.Linq;
using LaDOSE.Business.Interface;
using LaDOSE.Business.Provider.SmashProvider;
using LaDOSE.Entity;
using LaDOSE.Entity.Challonge;
using LaDOSE.Entity.Context;
using Microsoft.EntityFrameworkCore.Internal;

namespace LaDOSE.Business.Service
{
    public class PlayerService : BaseService<Player>, IPlayerService
    {

        public PlayerService(LaDOSEDbContext context) : base(context)
        {
        }

        public int GetIdBySmash(ParticipantType participantUser)
        {
            //var p2 = _context.Player.ToList();

            var p = _context.Player.FirstOrDefault(e => e.SmashId == participantUser.user.id);
            
            if (p == null)
            {
                p = _context.Player.FirstOrDefault(e => e.Gamertag.ToUpper() == participantUser.gamerTag.ToUpper());
                if (p != null)
                {
                    if (p.SmashId == null)
                    {
                        p.SmashId = participantUser.user.id;
                    }
                    return p.Id;
                }
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

        public int GetIdByName(ChallongeParticipent challongeParticipent)
        {
            if (string.IsNullOrEmpty(challongeParticipent.Name))
            {
                challongeParticipent.Name = "UNKNOWPLAYER";
            }
            var p = _context.Player.FirstOrDefault(e => e.Gamertag.ToUpper() == challongeParticipent.Name.ToUpper());
            if (p == null)
            {
                var entity = new Player()
                {
                    Gamertag = challongeParticipent.Name,
                    Name = challongeParticipent.Name,
                    ChallongeId = challongeParticipent.ChallongeId,
                };
                _context.Player.Add(entity);
                _context.SaveChanges();
                return entity.Id;
            }
            
            return p.Id;

        }
    }
}