using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using LaDOSE.DesktopApp.Avalonia.Utils;
using LaDOSE.DesktopApp.Avalonia.ViewModels;
using LaDOSE.DTO;
using LaDOSE.REST;
using ReactiveUI;
using Splat;

namespace LaDOSE.DesktopApp.Avalonia.ViewModels
{
    public class GamesViewModel : BaseViewModel
    {
   
        public string DisplayName => "Games";

        private GameDTO _currentGame;
        private List<GameDTO> _games;
        private RestService RestService { get; set; }
        public GamesViewModel(IScreen screen): base(screen,"Games")
        {
            this.RestService = Locator.Current.GetService<RestService>();
            this.Games=new List<GameDTO>();
            OnInitialize();
        }
        
        void OnInitialize()
        {
            LoadGames();
            this.CurrentGame = Games.First();
        }

        public void LoadGames()
        {
            var gameDtos = this.RestService.GetGames().OrderBy(e=>e.Order).ToList();
            this.Games = gameDtos;
            RaisePropertyChanged(nameof(this.Games));
        }

        public List<GameDTO> Games
        {
            get => _games;
            set
            {
                _games = value;
                RaisePropertyChanged(nameof(this.Games));
            }
        }

        public GameDTO CurrentGame
        {
            get => _currentGame;
            set
            {
                 _currentGame = value;
                RaisePropertyChanged(nameof(this.CurrentGame));
               
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