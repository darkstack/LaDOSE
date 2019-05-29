using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using LaDOSE.DTO;
using LaDOSE.REST;

namespace LaDOSE.DesktopApp.ViewModels
{


    public class TournamentResultViewModel : Screen
    {
        public override string DisplayName => "Tournament Result";

        private RestService RestService { get; set; }

        public TournamentResultViewModel(RestService restService)
        {
            this.RestService = restService;
            _selectedTournaments = new ObservableCollection<TournamentDTO>();
            Tournaments = new List<TournamentDTO>();
         
        }
        private ObservableCollection<TournamentDTO> _selectedTournaments;
        private TournamentsResultDTO _results;
        public List<TournamentDTO> Tournaments { get; set; }

        public TournamentsResultDTO Results
        {
            get => _results;
            set
            {
                _results = value;
                NotifyOfPropertyChange(() => Results);
            }
        }

        public ObservableCollection<TournamentDTO> SelectedTournaments
        {
            get { return _selectedTournaments; }
            set
            {
                _selectedTournaments = value;
                NotifyOfPropertyChange(() => SelectedTournaments);
            }
        }

        private GameDTO _selectedGame;

        public GameDTO SelectedGame
        {
            get { return _selectedGame; }
            set
            {
                _selectedGame = value;
                //TODO: QUICK AND DIRTY
                var resultForGame = this.Results.Results.Where(e => e.GameId == SelectedGame.Id).ToList();
                First = resultForGame.OrderByDescending(e=>e.Point).First().Player;
                NotifyOfPropertyChange(() => SelectedGame);
            }
        }
        private String _first;
        public String First
        {
            get { return _first; }
            set
            {
                _first = value;
                NotifyOfPropertyChange(() => First);
            }
        }
        protected override void OnInitialize()
        {
            LoadTournaments();
            base.OnInitialize();
        }

        public void LoadTournaments()
        {
            var tournamentDtos = this.RestService.GetTournaments().ToList();
            this.Tournaments = tournamentDtos;
            NotifyOfPropertyChange("Tournaments");
        }

        public void Select()
        {
            var tournamentsIds = SelectedTournaments.Select(e => e.Id).ToList();
            var resultsDto = this.RestService.GetResults(tournamentsIds);
            this.Results = resultsDto;
            
        }

        
    }

}