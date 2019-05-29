using System.Collections.Generic;
using System.Threading.Tasks;
using LaDOSE.Entity;
using LaDOSE.Entity.Wordpress;

namespace LaDOSE.Business.Interface
{
    public interface IWordPressService
    {
        WPEvent GetNextWpEvent();
        List<WPEvent> GetWpEvent();
        List<WPUser> GetBooking(int wpEventId, Game game);
        List<WPUser> GetBookingOptions(int wpEventId, Game game);
        bool UpdateBooking();
        string CreateChallonge(int gameId, int wpEventId, IList<WPUser> additionPlayers);

        Task<string> GetLastChallonge();


    }
}
    