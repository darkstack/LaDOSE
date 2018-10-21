using System.Collections.Generic;
using LaDOSE.Entity;
using LaDOSE.Entity.Wordpress;

namespace LaDOSE.Business.Interface
{
    public interface IEventService : IBaseService<Event>
    {
        bool CreateChallonge(int eventId, int wpEventId);
        List<WPUser> GetBooking(int eventId, int wpEventId, Game game);
    }
}