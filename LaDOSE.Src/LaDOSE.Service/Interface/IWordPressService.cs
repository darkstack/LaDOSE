﻿using System.Collections.Generic;
using LaDOSE.Entity;
using LaDOSE.Entity.Wordpress;

namespace LaDOSE.Business.Interface
{
    public interface IWordPressService
    {
        List<WPEvent> GetWpEvent();
        List<WPUser> GetBooking(int wpEventId, Game game);
        List<WPUser> GetBookingOptions(int wpEventId, Game game);
        bool UpdateBooking();
        bool CreateChallonge(int gameId, int wpEventId);
    }
}