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
        private ObservableCollection<WPUser> _optionalPlayers;

        private RestService RestService { get; set; }

        public WordPressViewModel(RestService restService)
        {
            this.RestService = restService;
            Players = new ObservableCollection<WPUser>();
            PlayersOptions = new ObservableCollection<WPUser>();
            OptionalPlayers = new ObservableCollection<WPUser>();
        }

        #region Auto Property

        protected override void OnInitialize()
        {
            base.OnInitialize();
            Task.Factory.StartNew(new Action(this.Load), TaskCreationOptions.LongRunning).ContinueWith(t => { },
                CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        public bool CanGenerate
        {
            get { return SelectedWpEvent != null && SelectedGame != null && Players?.Count() > 0; }
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

                Task.Factory.StartNew(LoadPlayers, TaskCreationOptions.LongRunning).ContinueWith(t => { },
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
                NotifyOfPropertyChange(() => Players);
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

        public ObservableCollection<WPUser> OptionalPlayers
        {
            get => _optionalPlayers;
            set
            {
                _optionalPlayers = value;
                NotifyOfPropertyChange(() => OptionalPlayers);
            }
        }

        public ObservableCollection<Game> GamesFound { get; set; }
        public List<Game> Games { get; set; }

        #endregion

        #region Commands

        public void UpdateDb()
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            var tsk = Task.Factory.StartNew(new Action(()=>this.RestService.RefreshDb()));
            tsk.ContinueWith(t =>
                {
                    MessageBox.Show(t.Exception.InnerException.Message);
                },
                CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted,
                TaskScheduler.FromCurrentSynchronizationContext());
            
               MessageBox.Show("Database updated");
            
        }

        public void Generate()
        {
            List<WPUser> test = new List<WPUser>();
            test = OptionalPlayers.ToList();
            var messageBoxText = this.RestService.CreateChallonge2(SelectedGame.Id, SelectedWpEvent.Id, test);

            if (messageBoxText != null && messageBoxText.Length > 0 && !messageBoxText.Contains("error"))
            {
                System.Diagnostics.Process.Start($"https://challonge.com/{messageBoxText}");
            }
            else
                MessageBox.Show("Didn't work :(");
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

        #endregion

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
                    var findUser = FindUser(SelectedWpEvent.Id, SelectedGame);
                    var findUser2 = FindUser(SelectedWpEvent.Id, SelectedGame,true);

                    findUser.ForEach((e) => this.Players.AddUI(e));
                    findUser2.ForEach((e) => this.PlayersOptions.AddUI(e));
                    //this.RestService.GetUsers(SelectedWpEvent.Id, SelectedGame.Id)
                    //    .ForEach((e) => this.Players.AddUI(e));
                    //this.RestService.GetUsersOptions(SelectedWpEvent.Id, SelectedGame.Id)
                    //    .ForEach((e) => this.PlayersOptions.AddUI(e));
                    NotifyOfPropertyChange(() => this.CanGenerate);
                }
        }

        private void Load()
        {
            Application.Current.Dispatcher.Invoke(() =>
                System.Windows.Input.Mouse.OverrideCursor = Cursors.Wait);
            GamesFound = new ObservableCollection<Game>();
            this.Games = this.RestService.GetGames();
            this.Events = this.RestService.GetEvents();

            NotifyOfPropertyChange("Events");
            Application.Current.Dispatcher.Invoke(() =>
                System.Windows.Input.Mouse.OverrideCursor = null);
        }

        public List<WPUser> FindUser(int wpEventId, Game game,bool optional = false)
        {

            string[] selectedGameWpId;
            selectedGameWpId = !optional ? game.WordPressTag.Split(';') : game.WordPressTagOs.Split(';');

            var currentWpEvent = this.Events.Where(e => e.Id == wpEventId).ToList();
            List<WPBooking> bookings = currentWpEvent.SelectMany(e => e.WpBookings).ToList();
            List<WPUser> users = new List<WPUser>();
            foreach (var booking in bookings)
            {
                var reservations = WpEventDeserialize.Parse(booking.Meta);
                if (reservations != null)
                {
                    var gamesReservation = reservations.Where(e => e.Valid).Select(e => e.Name);
                    if (selectedGameWpId.Any(e => gamesReservation.Contains(e)))
                    {
                        users.Add(booking.WpUser);
                    }
                }
            }

            return users;
        }
    }
}