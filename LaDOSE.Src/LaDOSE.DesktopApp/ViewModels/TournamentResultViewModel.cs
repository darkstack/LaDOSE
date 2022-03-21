using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
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
        //Dictionary<string, Dictionary<int, int>> _computedResult;

        #region Properties

        private string css = string.Empty;
            
                            //"strong { font-weight: 700;} ". +
                            // "a { color: #ff9024;}"+
                            // "body { color: #efefef;background-color: #141415; }" +
                            // ""+
                            // "a:hover, .entry-meta span a:hover, .comments-link a:hover, body.coldisplay2 #front-columns a:active {color: #cb5920;}"+
                            // "tr td { border: 1px dashed #3D3D3D;} ";
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

        private String _selectEventRegex;

        public String SelectEventRegex
        {
            get { return _selectEventRegex; }
            set
            {
                _selectEventRegex = value;
                NotifyOfPropertyChange(() => SelectEventRegex);
            }
        }
        private string _slug;
        public String Slug
        {
            get { return _slug; }
            set
            {
                _slug = value;
                NotifyOfPropertyChange(() => Slug);
            }
        }

        private String _html;

        public String Html
        {
            get { return $"<html><head><style>{this.css}</style></head><body>{HtmlContent}</body></html>";   }
            set
            {
                _html = value;
            }
        }
        private String _htmlContent;

        public String HtmlContent
        {
            get { return _htmlContent; }
            set
            {
                _htmlContent = value;
                NotifyOfPropertyChange(() => HtmlContent);
                NotifyOfPropertyChange(() => Html);
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

        public List<EventDTO> Events { get; set; }

        public TournamentsResultDTO Results
        {
            get => _results;
            set
            {
                _results = value;
                NotifyOfPropertyChange(() => Results);
            }
        }

        private ObservableCollection<EventDTO> _selectedEvents;

        public ObservableCollection<EventDTO> SelectedEvents
        {
            get { return _selectedEvents; }
            set
            {
                _selectedEvents = value;
                NotifyOfPropertyChange(() => SelectedEvents);
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
            _selectedEvents = new ObservableCollection<EventDTO>();
            Tournaments = new List<TournamentDTO>();
            Events = new List<EventDTO>();

        }


        protected override void OnInitialize()
        {
            var manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("LaDOSE.DesktopApp.Resources.css.css");
            using (var sr = new StreamReader(manifestResourceStream))
            {
                this.css = sr.ReadToEnd();
            }
            

            this.To = DateTime.Now;
            this.From = DateTime.Now.AddMonths(-1);
            this.SelectRegex = "Ranking";
            this.Slug = "ranking-1001";
            LoadTournaments();
            LoadEvents();
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

        public void LoadEvents()
        {
            WpfUtil.Await(() =>
            {
                var eventsDtos = this.RestService
                    .GetAllEvents().ToList();
                this.Events = eventsDtos;

                NotifyOfPropertyChange("Events");
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
                var tournamentsIds = SelectedEvents.Select(e => e.Id).ToList();
                var resultsDto = this.RestService.GetResults(tournamentsIds);
                this.Results = resultsDto;
                ComputeDataGrid();
                ComputeHtml();
            });
        }
        public void GetSmash()
        {
            WpfUtil.Await(() =>
            {
              
                var resultsDto = this.RestService.ParseSmash(Slug);
                if (!resultsDto)
                {
                    MessageBox.Show("Fail");
                }
            });
        }
        public void GetChallonge()
        {
            WpfUtil.Await(() =>
            {
                var ids = SelectedTournaments.Select(e => e.ChallongeId).ToList();
                var resultsDto = this.RestService.ParseChallonge(ids);
                if (!resultsDto)
                {
                    MessageBox.Show("Fail");
                }
            });
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
            var selectedTournaments = this.Tournaments.Where(e => Regex.IsMatch(e.Name, this.SelectRegex)).ToList();
            this.SelectedTournaments.Clear();
            if (selectedTournaments.Count > 0)
                selectedTournaments.ForEach(e => this.SelectedTournaments.AddUI(e));
        }
        public void SelectEvent()
        {
            var selectedEvents = this.Events.Where(e => Regex.IsMatch(e.Name, this.SelectEventRegex)).ToList();
            this.SelectedEvents.Clear();
            if (selectedEvents.Count > 0)
                selectedEvents.ForEach(e => this.SelectedEvents.AddUI(e));
        }
        //This could be simplified the Dictionary was for a previous usage, but i m too lazy to rewrite it. 
        private void ComputeDataGrid()
        {
            var resultsParticipents = this.Results.Participents.Select(e=>e.Name).Distinct(new CustomListExtension.EqualityComparer<String>((a, b) => a.ToUpperInvariant()== b.ToUpperInvariant())).OrderBy(e=>e).ToList();
            //At start the dictionnary was for some fancy dataviz things, but since the point are inside
            //i m to lazy to rewrite this functions (this is so ugly...)
            //_computedResult = ResultsToDataDictionary(resultsParticipents);

            StringBuilder sb = new StringBuilder();

            DataTable grid = new DataTable();
            var games = Results.Games.Distinct().OrderBy(e => e.Order).ToList();
            grid.Columns.Add("Players");
            games.ForEach(e => grid.Columns.Add(e.Name.Replace('.', ' '),typeof(Int32)));
            grid.Columns.Add("Total").DataType = typeof(Int32);


            for (int i = 0; i < resultsParticipents.Count; i++)
            {
                var dataRow = grid.Rows.Add();
                var resultsParticipent = resultsParticipents[i];
                int total = 0;
                dataRow["Players"] = resultsParticipent;


                for (int j = 0; j < games.Count; j++)
                {
                    var resultsGame = Results.Games[j];
                    var points = GetPlayerPoint(resultsParticipent, resultsGame.Id);
                    dataRow[resultsGame.Name.Replace('.', ' ')] = points!=0?(object) points:DBNull.Value;
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

        private void ComputeHtml()
        {
            
            StringBuilder sb = new StringBuilder();
            sb.Append("<table class=\"table table-responsive-md table-dark table-striped mt-lg-4 mt-3\">");

            int columns = 0;

            var distinct = Results.Results.Select(e => e.GameId).Distinct();

            var gamePlayed = Results.Games.Where(e=> distinct.Contains(e.Id));
            foreach (var game in gamePlayed)
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
                var span = 1;
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
                    var url = enumerable.FirstOrDefault().TournamentUrl;
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
            System.Windows.Clipboard.SetText(this.HtmlContent);
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