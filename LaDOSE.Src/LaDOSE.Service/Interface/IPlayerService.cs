using LaDOSE.Business.Provider.SmashProvider;
using LaDOSE.Entity;

namespace LaDOSE.Business.Interface
{
    public interface IPlayerService : IBaseService<Player>
    {
        int GetBySmash(ParticipantType participantUser);
    }
}