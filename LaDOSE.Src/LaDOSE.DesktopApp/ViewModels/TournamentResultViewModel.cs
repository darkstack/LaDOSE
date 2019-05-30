using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Forms;
using LaDOSE.DesktopApp.Utils;
using LaDOSE.DTO;
using LaDOSE.REST;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using Screen = Caliburn.Micro.Screen;

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

        private ObservableCollection<TournamentDTO> _selectedTournaments;

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
                List<ResultDTO> resultForGame = this.Results.Results.Where(e => e.GameId == SelectedGame.Id).ToList();
                First = resultForGame.OrderByDescending(e => e.Point).First().Player;
                SelectedGameResult = new ObservableCollection<ResultDTO>(resultForGame);
                NotifyOfPropertyChange(() => SelectedGame);
            }
        }

        private ObservableCollection<ResultDTO> _selectedGameResult;

        public ObservableCollection<ResultDTO> SelectedGameResult
        {
            get { return _selectedGameResult; }
            set
            {
                _selectedGameResult = value;
                NotifyOfPropertyChange(() => SelectedGameResult);
            }
        }

        private String _first;
        private DataTable _gridDataTable;

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

        public DataTable GridDataTable
        {
            get => _gridDataTable;
            set
            {
                _gridDataTable = value;
                NotifyOfPropertyChange(() => GridDataTable);
            }
        }

        public void Select()
        {
            var tournamentsIds = SelectedTournaments.Select(e => e.Id).ToList();
            var resultsDto = this.RestService.GetResults(tournamentsIds);
            this.Results = resultsDto;
            ComputeDataGrid();
        }

        private void ComputeDataGrid()
        {
            var resultsParticipents = this.Results.Participents.OrderBy(e => e.Name).ToList();
            var computed = ResultsToDataDictionary(resultsParticipents);

            StringBuilder sb = new StringBuilder();

            DataTable grid = new DataTable();
            grid.Columns.Add("Players");
            Results.Games.ForEach(e => grid.Columns.Add(e.Name.Replace('.',' ')));
            grid.Columns.Add("Total");


            for (int i = 0; i < resultsParticipents.Count; i++)
            {
                var dataRow = grid.Rows.Add();
                var resultsParticipent = resultsParticipents[i];
                int total = 0;
                dataRow["Players"] = resultsParticipent.Name;
                var gameDtos = Results.Games.Distinct().ToList();


                for (int j = 0; j < gameDtos.Count; j++)
                {
                    var resultsGame = Results.Games[j];
                    var dictionary = computed[resultsParticipent.Name];
                    if (dictionary.ContainsKey(resultsGame.Id))
                    {
                        int points = dictionary[resultsGame.Id];
                        dataRow[resultsGame.Name.Replace('.', ' ')] = points.ToString();
                        total += points;
                    }

                    else
                        dataRow[resultsGame.Name.Replace('.', ' ')] = null;
                }

                dataRow["Total"] = total.ToString();
            }

            this.GridDataTable = grid;
        }

        public void Export()
        {
            if (this.Results == null)
                return;


            ExportToCSV();
        }

        private void ExportToCSV()
        {
            SaveFileDialog sfDialog = new SaveFileDialog()
            {
                AddExtension = true
            };
            if (sfDialog.ShowDialog() == true)
            {
                var resultsParticipents = this.Results.Participents.OrderBy(e => e.Name).ToList();
                var computed = ResultsToDataDictionary(resultsParticipents);

                StringBuilder sb = new StringBuilder();


                sb.AppendLine(Results.Games.Aggregate("Player;", (current, t) => current + (t.Name + ";")));

                for (int i = 0; i < resultsParticipents.Count; i++)
                {
                    var entry = "";

                    var resultsParticipent = resultsParticipents[i];

                    entry = resultsParticipent.Name + ";";
                    var gameDtos = Results.Games.Distinct().ToList();
                    for (int j = 0; j < gameDtos.Count; j++)
                    {
                        var resultsGame = Results.Games[j];
                        var dictionary = computed[resultsParticipent.Name];
                        entry += dictionary.ContainsKey(resultsGame.Id)
                            ? dictionary[resultsGame.Id].ToString() + ";"
                            : ";";
                    }

                    sb.AppendLine(entry);
                }


                //string[][] resultCsv = new string[resultsParticipents.Count + 1][];
                //resultCsv[0] = new string[Results.Games.Count + 1];
                //resultCsv[0][0] = "Player";
                //for (int j = 0; j < Results.Games.Count; j++)
                //{
                //    resultCsv[0][j + 1] = Results.Games[j].Name;
                //}

                //for (int i = 0; i < resultsParticipents.Count; i++)
                //{
                //    resultCsv[i + 1] = new string[Results.Games.Count + 1];

                //    var resultsParticipent = resultsParticipents[i];

                //    resultCsv[i + 1][0] = resultsParticipent.Name;
                //    for (int j = 0; j < Results.Games.Count; j++)
                //    {
                //        var resultsGame = Results.Games[j];
                //        var dictionary = computed[resultsParticipent.Name];
                //        if (dictionary.ContainsKey(resultsGame.Id))
                //        {
                //            var i1 = dictionary[resultsGame.Id];
                //            resultCsv[i + 1][j + 1] = i1.ToString();
                //        }
                //    }
                //}

                //Save 
                File.WriteAllText(sfDialog.FileName, sb.ToString());
            }
        }

        private Dictionary<string, Dictionary<int, int>> ResultsToDataDictionary(
            List<ParticipentDTO> resultsParticipents)
        {
            var computed = new Dictionary<string, Dictionary<int, int>>();


            foreach (var participent in resultsParticipents)
            {
                computed.Add(participent.Name, new Dictionary<int, int>());
            }

            foreach (var game in Results.Games)
            {
                var results = this.Results.Results.Where(e => e.GameId == game.Id).ToList();
                foreach (var result in results)
                {
                    var dictionary = computed[result.Player];
                    if (dictionary.ContainsKey(result.GameId))
                        dictionary[game.Id] += result.Point;
                    else
                    {
                        dictionary.Add(game.Id, result.Point);
                    }
                }
            }

            return computed;
        }
    }
}