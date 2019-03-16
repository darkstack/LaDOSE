using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using LaDOSE.DTO;
using LaDOSE.REST;

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
            this.CurrentGame = Games.First();
            base.OnInitialize();
        }

        public void LoadGames()
        {
            var gameDtos = this.RestService.GetGames().OrderBy(e=>e.Order).ToList();
            this.Games = gameDtos;
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
            LoadGames();
               
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