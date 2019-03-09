using System.Collections.Generic;
using Caliburn.Micro;
using LaDOSE.DesktopApp.Services;
using LaDOSE.DTO;

namespace LaDOSE.DesktopApp.ViewModels
{
    public class GameViewModel : Screen
    {
        public override string DisplayName => "Games";

        private Game _currentGame;
        private List<Game> _games;
        private RestService RestService { get; set; }
        public GameViewModel(RestService restService)
        {
            this.RestService = restService;
            this.Games=new List<Game>();
        }

        public void LoadGames()
        {
            this.Games.Clear();
            this.Games = this.RestService.GetGames();
            NotifyOfPropertyChange("Games");
        }

        public List<Game> Games
        {
            get => _games;
            set
            {
                _games = value;
                NotifyOfPropertyChange(()=>this.Games);
            }
        }

        public Game CurrentGame
        {
            get => _currentGame;
            set
            {
                 _currentGame = value;
                NotifyOfPropertyChange(()=>CurrentGame);
            }
        }

        public void Update()
        {
            this.RestService.UpdateGame(this.CurrentGame);
            this.Games = RestService.GetGames();
        }
        public void AddGame()
        {
            var item = new Game();
            this.Games.Add(item);
            this.CurrentGame = item;
        }
    }
}