using LaDOSE.Business.Provider.SmashProvider;
using LaDOSE.Entity;
using LaDOSE.Entity.Challonge;

namespace LaDOSE.Business.Interface
{
    public interface IPlayerService : IBaseService<Player>
    {
        int GetIdBySmash(ParticipantType participantUser);
        int GetIdByName(ChallongeParticipent challongeParticipent);
    }
}