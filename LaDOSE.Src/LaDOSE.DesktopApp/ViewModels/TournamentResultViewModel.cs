using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        #region Properties
        private String _selectRegex;

        public String SelectRegex
        {
            get { return _selectRegex; }
            set
            {
                _selectRegex = value;
                NotifyOfPropertyChange(() => SelectRegex);
            }
        }



        private DateTime _from;
    
        public DateTime From
        {
            get { return _from; }
            set
            {
                _from = value;
                NotifyOfPropertyChange(() => From);
            }
        }

        private DateTime _to;
        public DateTime To
        {
            get { return _to; }
            set
            {
                _to = value;
                NotifyOfPropertyChange(() => To);
            }
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

        #endregion
        public TournamentResultViewModel(RestService restService)
        {
            this.RestService = restService;
            _selectedTournaments = new ObservableCollection<TournamentDTO>();
            Tournaments = new List<TournamentDTO>();
        }

    

        protected override void OnInitialize()
        {
            this.To=DateTime.Now;
            this.From = DateTime.Now.AddMonths(-1);
            this.SelectRegex = "Ranking";
            LoadTournaments();
            base.OnInitialize();
        }

        public void LoadTournaments()
        {
            WpfUtil.Await(() =>
            {
                var tournamentDtos = this.RestService
                    .GetTournaments(new TimeRangeDTO() {From = this.From, To = this.To}).ToList();
                this.Tournaments = tournamentDtos;

                NotifyOfPropertyChange("Tournaments");
            });

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
            WpfUtil.Await(() =>
            {
                var tournamentsIds = SelectedTournaments.Select(e => e.Id).ToList();
                var resultsDto = this.RestService.GetResults(tournamentsIds);
                this.Results = resultsDto;
                ComputeDataGrid();
            });

        }

        public void SelectYear()
        {
            this.To = DateTime.Now;
            this.From = new DateTime(DateTime.Now.Year,1,1);

        }
        public void SelectMonth()
        {
            this.To = DateTime.Now;
            this.From = DateTime.Now.AddMonths(-1);
        }
        public void SelectRegexp()
        {
            var selectedTournaments = this.Tournaments.Where(e => Regex.IsMatch(e.Name, this.SelectRegex)).ToList();
            this.SelectedTournaments.Clear();
            if(selectedTournaments.Count>0)
                selectedTournaments.ForEach(e=>this.SelectedTournaments.AddUI(e));
            
        }

        private void ComputeDataGrid()
        {
            var resultsParticipents = this.Results.Participents.OrderBy(e => e.Name).ToList();
            var computed = ResultsToDataDictionary(resultsParticipents);

            StringBuilder sb = new StringBuilder();

            DataTable grid = new DataTable();
            var games = Results.Games.Distinct().OrderBy(e=>e.Order).ToList();
            grid.Columns.Add("Players");
            games.ForEach(e => grid.Columns.Add(e.Name.Replace('.',' ')));
            grid.Columns.Add("Total").DataType = typeof(Int32);


            for (int i = 0; i < resultsParticipents.Count; i++)
            {
                var dataRow = grid.Rows.Add();
                var resultsParticipent = resultsParticipents[i];
                int total = 0;
                dataRow["Players"] = resultsParticipent.Name;
                


                for (int j = 0; j < games.Count; j++)
                {
                    var resultsGame = Results.Games[j];
                    var dictionary = computed[resultsParticipent.Name];
                    if (dictionary.ContainsKey(resultsGame.Id))
                    {
                        int points = dictionary[resultsGame.Id];
                        dataRow[resultsGame.Name.Replace('.', ' ')] = points;
                        total += points;
                    }

                    else
                        dataRow[resultsGame.Name.Replace('.', ' ')] = null;
                }
                
                dataRow["Total"] = total;
            }

            grid.DefaultView.Sort = "Total DESC";
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

            
            if (this.GridDataTable != null)
            {
                var dataTable = this.GridDataTable.DefaultView.ToTable();
                SaveFileDialog sfDialog = new SaveFileDialog()
                {
                    Filter = "Csv Files (*.csv)|*.csv|All Files (*.*)|*.*",
                    AddExtension = true
                };
                if (sfDialog.ShowDialog() == true)
                {
                    StringBuilder sb = new StringBuilder();

                    IEnumerable<string> columnNames = dataTable.Columns.Cast<DataColumn>()
                        .Select(column => column.ColumnName);
                    sb.AppendLine(string.Join(";", columnNames));

                    foreach (DataRow row in dataTable.Rows)
                    {
                        //EXCEL IS A BITCH
                        IEnumerable<string> fields = row.ItemArray.Select(field =>
                            string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                        sb.AppendLine(string.Join(";", fields));
                    }

                    File.WriteAllText(sfDialog.FileName, sb.ToString());
                }
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