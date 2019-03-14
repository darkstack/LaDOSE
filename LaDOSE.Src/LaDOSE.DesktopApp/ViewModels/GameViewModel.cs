using System.Collections.Generic;
using Caliburn.Micro;
using LaDOSE.DesktopApp.Services;
using LaDOSE.DTO;

namespace LaDOSE.DesktopApp.ViewModels
{
    public class GameViewModel : Screen
    {
        public override string DisplayName => "Games";

        private GameDTO _currentGame;
        private List<GameDTO> _games;
        private RestService RestService { get; set; }
        public GameViewModel(RestService restService)
        {
            this.RestService = restService;
            this.Games=new List<GameDTO>();

        }

        protected override void OnInitialize()
        {
            LoadGames();
            base.OnInitialize();
        }

        public void LoadGames()
        {
            this.Games.Clear();
            this.Games = this.RestService.GetGames();
            NotifyOfPropertyChange("Games");
        }

        public List<GameDTO> Games
        {
            get => _games;
            set
            {
                _games = value;
                NotifyOfPropertyChange(()=>this.Games);
            }
        }

        public GameDTO CurrentGame
        {
            get => _currentGame;
            set
            {
                 _currentGame = value;
                NotifyOfPropertyChange(()=>CurrentGame);
                NotifyOfPropertyChange(() => CanDeleteGame);
            }
        }

        public void Update()
        {
            this.RestService.UpdateGame(this.CurrentGame);
            this.Games = RestService.GetGames();
        }
        public void AddGame()
        {
            var item = new GameDTO();
            this.RestService.UpdateGame(item);
            LoadGames();
        }
        public void DeleteGame()
        {
            
            this.RestService.DeleteGame(this.CurrentGame.Id);
            LoadGames();
        }

        public bool CanDeleteGame => CurrentGame != null;
    }
}