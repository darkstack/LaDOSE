using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Avalonia.Collections;
using Avalonia.Controls;
using LaDOSE.DesktopApp.Avalonia.Utils;
using LaDOSE.DTO;
using LaDOSE.REST;
using ReactiveUI;
using Splat;

namespace LaDOSE.DesktopApp.Avalonia.ViewModels
{
    public class TournamentResultViewModel : BaseViewModel
    {
        public string DisplayName => "Tournament Result";

        private RestService? RestService { get; set; }
        //Dictionary<string, Dictionary<int, int>> _computedResult;

        #region Properties

        private string css = string.Empty;
     
        private string? _selectRegex;

        public string? SelectRegex
        {
            get { return _selectRegex; }
            set
            {
                _selectRegex = value;
                RaisePropertyChanged(nameof(SelectRegex));
            }
        }

        private string? _selectEventRegex;

        public string? SelectEventRegex
        {
            get { return _selectEventRegex; }
            set
            {
                _selectEventRegex = value;
                RaisePropertyChanged(nameof(SelectEventRegex));
            }
        }
        private string? _slug;
        public string? Slug
        {
            get { return _slug; }
            set
            {
                _slug = value;
                RaisePropertyChanged(nameof(Slug));
            }
        }

        private string? _html;

        public string? Html
        {
            get { return $"<html><head><style>{this.css}</style></head><body>{HtmlContent}</body></html>";   }
            set
            {
                _html = value;
            }
        }
        private string? _htmlContent;

        public string? HtmlContent
        {
            get { return _htmlContent; }
            set
            {
                _htmlContent = value;
                RaisePropertyChanged(nameof(HtmlContent));
                RaisePropertyChanged(nameof(Html));
            }
        }



        private DateTimeOffset _from;

        public DateTimeOffset From
        {
            get { return _from; }
            set
            {
                _from = value;
                RaisePropertyChanged(nameof(From));
            }
        }

        private DateTimeOffset _to;

        public DateTimeOffset To
        {
            get { return _to; }
            set
            {
                _to = value;
                RaisePropertyChanged(nameof(To));
            }
        }


        private TournamentsResultDTO? _results;
        public List<TournamentDTO> Tournaments { get; set; }

        public List<EventDTO> Events { get; set; }

        public TournamentsResultDTO? Results
        {
            get => _results;
            set
            {
                _results = value;
                RaisePropertyChanged(nameof(Results));
            }
        }

        private ObservableCollection<EventDTO> _selectedEvents;

        public ObservableCollection<EventDTO> SelectedEvents
        {
            get { return _selectedEvents; }
            set
            {
                _selectedEvents = value;
                RaisePropertyChanged(nameof(SelectedEvents));
            }
        }

        private ObservableCollection<TournamentDTO> _selectedTournaments;

        public ObservableCollection<TournamentDTO> SelectedTournaments
        {
            get { return _selectedTournaments; }
            set
            {
                _selectedTournaments = value;
                RaisePropertyChanged(nameof(SelectedTournaments));
            }
        }

        private GameDTO? _selectedGame;


        public GameDTO? SelectedGame
        {
            get { return _selectedGame; }
            set
            {
                _selectedGame = value;
                //TODO: QUICK AND DIRTY
                List<ResultDTO> resultForGame = this.Results.Results.Where(e => e.GameId == SelectedGame?.Id).ToList();
                if (resultForGame.Any())
                {
                    First = resultForGame.OrderByDescending(e => e.Point).First().Player;
                    SelectedGameResult = new ObservableCollection<ResultDTO>(resultForGame);
                }
                
                RaisePropertyChanged(nameof(SelectedGame));
            }
        }

        private ObservableCollection<ResultDTO>? _selectedGameResult;

        public ObservableCollection<ResultDTO>? SelectedGameResult
        {
            get { return _selectedGameResult; }
            set
            {
                _selectedGameResult = value;
                RaisePropertyChanged(nameof(SelectedGameResult));
            }
        }

        private string? _first;
        private DataTable? _gridDataTable;
        private string? _error;

        public string? First
        {
            get { return _first; }
            set
            {
                _first = value;
                RaisePropertyChanged(nameof(First));
            }
        }

        #endregion

        public TournamentResultViewModel(IScreen hostScreen):base(hostScreen,"Tournament")
        {
            this.RestService = Locator.Current.GetService<RestService>();;
            _selectedTournaments = new ObservableCollection<TournamentDTO>();
            _selectedEvents = new ObservableCollection<EventDTO>();
            Tournaments = new List<TournamentDTO>();
            Events = new List<EventDTO>();
            OnInitialize();
        }


        protected void OnInitialize()
        {
            // var manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LaDOSE.DesktopApp.Resources.css.css");
            // using (var sr = new StreamReader(manifestResourceStream))
            // {
            //     this.css = sr.ReadToEnd();
            // }
            

            this.To = new DateTimeOffset(DateTime.Now);
            this.From = new DateTimeOffset(DateTime.Now.AddMonths(-1));
            this.SelectRegex = "Ranking";
            this.SelectEventRegex = @"Ranking #10\d{2}";
            this.Slug = "ranking-1001";
            
            LoadTournaments();
            LoadEvents();
        }

        public void LoadTournaments()
        {
  
                // var tournamentDtos = this.RestService
                //     .GetTournaments(new TimeRangeDTO() {From = this.From, To = this.To}).ToList();
                // this.Tournaments = tournamentDtos;

                RaisePropertyChanged(nameof(Tournaments));
        
        }

        public void LoadEvents()
        {
    
                List<EventDTO> eventsDtos = this.RestService
                    .GetAllEvents().ToList();
                this.Events = eventsDtos;

                RaisePropertyChanged(nameof(Events));
           
        }

        public DataTable? GridDataTable
        {
            get => _gridDataTable;
            set
            {
                _gridDataTable = value;
                RaisePropertyChanged(nameof(GridDataTable));
                RaisePropertyChanged(nameof(GridDataTableView));
            }
        }
        public DataView? GridDataTableView
        {
            get
            {
                DataView gridDataTableView = _gridDataTable?.AsDataView();
                return gridDataTableView;
            }
        }

        public void Select()
        {
      
                List<int> tournamentsIds = SelectedEvents.Select(e => e.Id).ToList();
                TournamentsResultDTO? resultsDto = this.RestService.GetResults(tournamentsIds);
                this.Results = resultsDto;
                ComputeDataGrid();
                ComputeHtml();
        
        }
        public void GetSmash()
        {

              
                bool resultsDto = this.RestService.ParseSmash(Slug);
                if (!resultsDto)
                {
                    Error = "Error getting Smash";
                }
          
        }

        public string? Error
        {
            get => _error;
            set
            {
                if (value == _error) return;
                _error = value;
                RaisePropertyChanged();
            }
        }

        public void GetChallonge()
        {
           
                List<int> ids = SelectedTournaments.Select(e => e.ChallongeId).ToList();
                bool resultsDto = this.RestService.ParseChallonge(ids);
                if (!resultsDto)
                {
                    Error = "Fail";
                }
        }

        public void UpdateEvent()
        {
            LoadEvents();
        }

        public void SelectYear()
        {
            this.To = DateTime.Now;
            this.From = new DateTime(DateTime.Now.Year, 1, 1);
        }

        public void SelectMonth()
        {
            this.To = DateTime.Now;
            this.From = DateTime.Now.AddMonths(-1);
        }

        public void SelectRegexp()
        {
            List<TournamentDTO> selectedTournaments = this.Tournaments.Where(e => Regex.IsMatch(e.Name, this.SelectRegex)).ToList();
            this.SelectedTournaments.Clear();
            if (selectedTournaments.Count > 0)
                selectedTournaments.ForEach(e => this.SelectedTournaments.Add(e));
        }
        public void SelectEvent()
        {
            List<EventDTO> selectedEvents = this.Events.Where(e => Regex.IsMatch(e.Name, this.SelectEventRegex)).ToList();
            this.SelectedEvents.Clear();
            if (selectedEvents.Count > 0)
                selectedEvents.ForEach(e => this.SelectedEvents.Add(e));
        }
        //This could be simplified the Dictionary was for a previous usage, but i m too lazy to rewrite it. 
        private void ComputeDataGrid()
        {
            List<string> resultsParticipents = this.Results.Participents.Select(e=>e.Name).Distinct(new CustomListExtension.EqualityComparer<String>((a, b) => a.ToUpperInvariant()== b.ToUpperInvariant())).OrderBy(e=>e).ToList();
            //At start the dictionnary was for some fancy dataviz things, but since the point are inside
            //i m to lazy to rewrite this functions (this is so ugly...)
            //_computedResult = ResultsToDataDictionary(resultsParticipents);

            StringBuilder sb = new StringBuilder();

            DataTable? grid = new DataTable();
            List<GameDTO> games = Results.Games.Distinct().OrderBy(e => e.Order).ToList();
            grid.Columns.Add("Players");
            games.ForEach(e => grid.Columns.Add(e.Name.Replace('.', ' '),typeof(Int32)));
            grid.Columns.Add("Total").DataType = typeof(Int32);


            for (int i = 0; i < resultsParticipents.Count; i++)
            {
                DataRow dataRow = grid.Rows.Add();
                string resultsParticipent = resultsParticipents[i];
                int total = 0;
                dataRow["Players"] = resultsParticipent;


                for (int j = 0; j < games.Count; j++)
                {
                    GameDTO? resultsGame = Results.Games[j];
                    int points = GetPlayerPoint(resultsParticipent, resultsGame.Id);
                    var o = dataRow[resultsGame.Name.Replace('.', ' ')];
                    dataRow[resultsGame.Name.Replace('.', ' ')] = points!=0?points:0;
                    total += points;
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
            // if (this.GridDataTable != null)
            // {
            //     var dataTable = this.GridDataTable.DefaultView.ToTable();
            //     SaveFileDialog sfDialog = new SaveFileDialog()
            //     {
            //         Filter = "Csv Files (*.csv)|*.csv|All Files (*.*)|*.*",
            //         AddExtension = true
            //     };
            //     if (sfDialog.ShowDialog() == true)
            //     {
            //         StringBuilder sb = new StringBuilder();
            //
            //         IEnumerable<string> columnNames = dataTable.Columns.Cast<DataColumn>()
            //             .Select(column => column.ColumnName);
            //         sb.AppendLine(string.Join(";", columnNames));
            //
            //         foreach (DataRow row in dataTable.Rows)
            //         {
            //             //EXCEL IS A BITCH
            //             IEnumerable<string> fields = row.ItemArray.Select(field =>
            //                 string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
            //             sb.AppendLine(string.Join(";", fields));
            //         }
            //
            //         File.WriteAllText(sfDialog.FileName, sb.ToString());
            //     }
            // }
        }

        private void ComputeHtml()
        {
            
            StringBuilder sb = new StringBuilder();
            sb.Append("<table class=\"table table-responsive-md table-dark table-striped mt-lg-4 mt-3\">");

            int columns = 0;

            IEnumerable<int> distinct = Results.Results.Select(e => e.GameId).Distinct();

            IOrderedEnumerable<GameDTO> gamePlayed = Results.Games.Where(e=> distinct.Contains(e.Id)).OrderBy(e=>e.Order);
            foreach (GameDTO game in gamePlayed)
            {
                List<ResultDTO> enumerable = Results.Results.Where(r => r.GameId == game.Id).ToList();
                List<string> top3 = enumerable.OrderBy(e => e.Rank).Take(3).Select(e => e.Player).ToList();
                if (top3.Count == 0)
                {
                    continue;
                }

                if (columns % 2 == 0)
                {
                    sb.Append("<tr>");
                }
                columns++;
                int span = 1;
                if (columns == gamePlayed.Count())
                {
                    if (columns % 2 != 0)
                    {
                        span = 2;
                    }
                }
                sb.Append($"<td colspan=\"{span}\" width=\"50%\">" +
                          "<span style=\"color: #ff0000;\">" +
                          $"<strong>{game.LongName} ({Results.Results.Count(e => e.GameId == game.Id)} participants) :</strong>" +
                          "</span>");
                
                if (top3.Count >= 3)
                {
                    sb.AppendLine($"<br> 1/ {top3[0]}<br> 2/ {top3[1]}<br> 3/ {top3[2]} <br>");
                    //<a href=\"https://challonge.com/fr/{enumerable.First().TournamentUrl}\" target=\"_blank\">https://challonge.com/fr/{enumerable.First().TournamentUrl}</a>
                    string url = enumerable.FirstOrDefault()?.TournamentUrl;
                    url = url.Replace(" ", "-");
                    url = url.Replace(".", "-");
                    sb.AppendLine($"<a href=\"https://smash.gg/tournament/ranking-1002/event/{url}\" target=\"_blank\">Voir le Bracket</p></td>");
                }

                
                if (columns % 2 == 0)
                {
                    sb.Append("</tr>");
                }


            }

            sb.Append("</table>");


           this.HtmlContent = sb.ToString();
              
        }
        public void CopyHtml()
        {
            // System.Windows.Clipboard.SetText(this.HtmlContent);
        }

        private int GetPlayerPoint(string name, int gameid)
        {
            return Results.Results.Where(e => e.GameId == gameid && e.Player.ToUpperInvariant() == name.ToUpperInvariant()).Sum(e=>e.Point);
        }

    //    private Dictionary<string, Dictionary<int, int>> ResultsToDataDictionary(
    //        List<ParticipentDTO> resultsParticipents)
    //    {
    //        var computed = new Dictionary<string, Dictionary<int, int>>();


    //        foreach (var participent in resultsParticipents)
    //        {
    //            computed.Add(participent.Name, new Dictionary<int, int>());
    //        }

    //        foreach (var game in Results.Games)
    //        {
    //            var results = Results.Results.Where(e => e.GameId == game.Id).ToList();
    //            foreach (var result in results)
    //            {
    //                var dictionary = computed[result.Player];
    //                if (dictionary.ContainsKey(result.GameId))
    //                    dictionary[game.Id] += result.Point;
    //                else
    //                {
    //                    dictionary.Add(game.Id, result.Point);
    //                }
    //            }
    //        }
            
    //        MergeDuplicates(resultsParticipents, computed);

    //        return computed;
    //    }

    //    private static void MergeDuplicates(List<ParticipentDTO> resultsParticipents, Dictionary<string, Dictionary<int, int>> computed)
    //    {
    //        var duplicates = computed.Keys.ToList().GroupBy(x => x.ToUpperInvariant()).Where(x => x.Count() > 1)
    //            .Select(x => x.Key)
    //            .ToList();
    //        if (duplicates.Count > 0)
    //        {
    //            foreach (var duplicate in duplicates)
    //            {
    //                var lines = computed.Where(e => e.Key.ToUpperInvariant() == duplicate).ToList();
    //                for (int i = lines.Count(); i > 1; --i)
    //                {
    //                    var result = lines[--i];
    //                    foreach (var games in result.Value.Keys)
    //                    {
    //                        if (lines[0].Value.ContainsKey(games))
    //                            lines[0].Value[games] += result.Value[games];
    //                        else
    //                            lines[0].Value.Add(games, result.Value[games]);
    //                    }

    //                    computed.Remove(result.Key);
    //                    resultsParticipents.Remove(resultsParticipents.First(e => e.Name == result.Key));
    //                }
    //            }
    //        }
    //    }
    }
}