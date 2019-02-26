using System.Collections.Generic;
using Caliburn.Micro;
using LaDOSE.DesktopApp.Services;
using LaDOSE.DTO;

namespace LaDOSE.DesktopApp.ViewModels
{
    public class GameViewModel : Screen
    {
        private RestService RestService { get; set; }
        public GameViewModel(RestService restService)
        {
            this.RestService = restService;
        }

        public void LoadGames()
        {
            this.Games = this.RestService.GetGames();
            NotifyOfPropertyChange("Games");
        }

        public List<Game> Games { get; set; }
    }
}