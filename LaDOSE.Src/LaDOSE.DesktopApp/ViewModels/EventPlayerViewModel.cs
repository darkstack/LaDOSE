using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Caliburn.Micro;
using LaDOSE.DesktopApp.Utils;
using LaDOSE.DTO;
using LaDOSE.REST;
using Microsoft.Win32;
using RestSharp.Serialization.Json;

namespace LaDOSE.DesktopApp.ViewModels
{
    public class EventPlayerViewModel : Screen
    {
        public override string DisplayName => "EventPlayers";


        public class KiouzeFile
        {
            public String player { get; set; }
            public String character { get; set; }
        }

        public ObservableCollection<KiouzeFile> Data { get; set; }

        private List<string> _playersList;
        private List<string> _charList;
        private string _chars { get; set; }
        public string Slug { get; set; }
        
        private RestService RestService { get; set; }
        public EventPlayerViewModel(RestService restService)
        {
            this.RestService = restService;
            Data = new ObservableCollection<KiouzeFile>();
            this.Slug = "tag-team-tuesdays-umvc3-team-tournament-2";
            this.Chars = "akuma;blanka;boxer;cammy;chunli;claw;deejay;dhalsim;dictator;feilong;guile;honda;ken;ryu;sagat;thawk;zangief;o_blanka;o_boxer;o_cammy;o_chunli;o_claw;o_deejay;o_dhalsim;o_dictator;o_feilong;o_guile;o_honda;o_ken;o_ryu;o_sagat;o_thawk;o_zangief";

        }

        protected override void OnInitialize()
        {
        
            base.OnInitialize();
        }

        public void GetPlayers()
        {
            WpfUtil.Await(() =>
            {

                var resultsDto = this.RestService.GetPlayers(this.Slug);
                Players = resultsDto;
                Players.ForEach(p=> Data.AddUI(new KiouzeFile(){player = p}));
            });
        }

        public void Export()
        {
       
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                JsonSerializer p = new JsonDeserializer();
                var json = p.Serialize(Data.ToList());
                File.WriteAllText(saveFileDialog.FileName, json);
            }
        }
        public List<String> Players
        {
            get => _playersList;
            set
            {
                _playersList = value;
                NotifyOfPropertyChange(() => this.Players);
            }
        }
        public List<String> CharList
        {
            get => _charList;
            set
            {
                _charList = value;
                NotifyOfPropertyChange(() => this.CharList);
            }
        }


        public String Chars
        {
            get => _chars;
            set
            {
                _chars = value;
                _charList = _chars.Split(';').ToList();
                NotifyOfPropertyChange(()=>this.Players);
                NotifyOfPropertyChange(()=>this.CharList);
            }
        }




    }
}