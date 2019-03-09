using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Caliburn.Micro;
using LaDOSE.DesktopApp.Services;
using LaDOSE.DesktopApp.Utils;
using LaDOSE.DTO;
using Action = System.Action;

namespace LaDOSE.DesktopApp.ViewModels
{
    public class WordPressViewModel : Screen
    {
        public override string DisplayName => "Events";
        private WPEvent _selectedWpEvent;
        private Game _selectedGame;
        private ObservableCollection<WPUser> _players;
        private ObservableCollection<WPUser> _playersOptions;
        
        private RestService RestService { get; set; }

        public WordPressViewModel(RestService restService)
        {
            this.RestService = restService;
            Players = new ObservableCollection<WPUser>();
            PlayersOptions = new ObservableCollection<WPUser>();
        }

        public void LoadEvents()
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            var tsk = Task.Factory.StartNew(Load);
            tsk.ContinueWith(t =>
                {
                    MessageBox.Show(t.Exception.InnerException.Message);
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                },
                CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Load()
        {
            GamesFound = new ObservableCollection<Game>();
            this.Games = this.RestService.GetGames();
            this.Events = this.RestService.GetEvents();

            NotifyOfPropertyChange("Events");
            Application.Current.Dispatcher.Invoke(() =>
                System.Windows.Input.Mouse.OverrideCursor = null);
        }


        public bool CanGenerate
        {
            get { return SelectedWpEvent != null && SelectedGame != null; }
        }


        public List<WPEvent> Events { get; set; }

        public WPEvent SelectedWpEvent
        {
            get => _selectedWpEvent;
            set
            {
                _selectedWpEvent = value;
                SelectedGame = null;
                ParseGame(_selectedWpEvent);
              
            }
        }

        public Game SelectedGame
        {
            get => _selectedGame;
            set
            {
                _selectedGame = value;
             
                Players.Clear();
                PlayersOptions.Clear();

                Task.Factory.StartNew(LoadPlayers,TaskCreationOptions.LongRunning).ContinueWith(t =>
                    {

                    },
                    CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted,
                    TaskScheduler.FromCurrentSynchronizationContext());
                NotifyOfPropertyChange(() => SelectedGame);
                NotifyOfPropertyChange(() => this.CanGenerate);
                NotifyOfPropertyChange(() => Players);
                NotifyOfPropertyChange(() => PlayersOptions);
            }
        }



        public ObservableCollection<WPUser> Players
        {
            get => _players;
            set
            {
                _players = value;
                NotifyOfPropertyChange(()=>Players);
            }
        }

        public ObservableCollection<WPUser> PlayersOptions
        {
            get => _playersOptions;
            set
            {
                _playersOptions = value;
                NotifyOfPropertyChange(() => PlayersOptions);
            }
        }


        public ObservableCollection<Game> GamesFound { get; set; }
        public List<Game> Games { get; set; }

        private void ParseGame(WPEvent selectedWpEvent)
        {
            var reservation = SelectedWpEvent.WpBookings.FirstOrDefault();
            var games = WpEventDeserialize.Parse(reservation.Meta);
            GamesFound.Clear();

            if (games != null)
            {
                foreach (string wpTag in games.Select(e => e.Name))
                {
                    var foundGame = Games.FirstOrDefault(e =>
                        e.WordPressTag != null && e.WordPressTag.Split(';').Contains(wpTag));
                    if (foundGame != null)
                    {
                        if (!GamesFound.Contains(foundGame))
                        {
                            GamesFound.Add(foundGame);
                        }
                    }
                }
            }

            NotifyOfPropertyChange(() => GamesFound);
        }

        private void LoadPlayers()
        {
     
            if (SelectedWpEvent != null)
                if (SelectedGame != null)
                {
                  this.RestService.GetUsers(SelectedWpEvent.Id, SelectedGame.Id).ForEach((e) => this.Players.AddUI(e));
                  this.RestService.GetUsersOptions(SelectedWpEvent.Id, SelectedGame.Id).ForEach((e) => this.PlayersOptions.AddUI(e));

                }
                    
        }

        public void UpdateDb()
        {
            if (this.RestService.RefreshDb())
                MessageBox.Show("DataBaseUpdated");
            else
                MessageBox.Show("Update Failed");
        }

        public void Generate()
        {
            if (this.RestService.CreateChallonge(SelectedGame.Id, SelectedWpEvent.Id))
                MessageBox.Show("Challonge Created");
            else
                MessageBox.Show("Didn't worl :(");
        }

    }
}