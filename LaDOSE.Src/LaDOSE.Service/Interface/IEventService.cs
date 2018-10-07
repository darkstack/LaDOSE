using LaDOSE.Entity;

namespace LaDOSE.Business.Interface
{
    public interface IEventService : IBaseService<Event>
    {
        bool CreateChallonge(int dto);
    }
}