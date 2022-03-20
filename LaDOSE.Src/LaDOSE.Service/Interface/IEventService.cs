using LaDOSE.Entity;
using LaDOSE.Entity.Wordpress;

namespace LaDOSE.Business.Interface
{
    public interface IEventService : IBaseService<Event>
    {

        Event GetBySlug(string tournamentSlug);
        Event GetByName(string name);

    }
}