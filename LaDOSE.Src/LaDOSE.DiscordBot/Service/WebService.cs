using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChallongeCSharpDriver;
using ChallongeCSharpDriver.Caller;
using ChallongeCSharpDriver.Core.Queries;
using ChallongeCSharpDriver.Core.Results;
using ChallongeCSharpDriver.Main;
using ChallongeCSharpDriver.Main.Objects;
using LaDOSE.DTO;
using LaDOSE.REST;
using RestSharp.Authenticators;

namespace LaDOSE.DiscordBot.Service
{
    public class WebService
    {
        private RestService restService;

        public RestService RestService => restService;

        public WebService(Uri uri,string user,string password)
        {
          restService = new RestService();
          restService.Connect(uri,user,password);
        }

        private void CheckToken()
        {
            
         
        }



        public String GetInscrits()
        {
            var wpEventDto = restService.GetNextEvent();
            var wpBookingDtos = wpEventDto.WpBookings;
            List<String> player= new List<string>();
            wpBookingDtos.OrderBy(e=>e.WpUser.Name).ToList().ForEach(e=> player.Add(e.WpUser.Name));
            return $"Les Joueurs inscrits pour {wpEventDto.Name} {string.Join(", ", player)}";
        }
        public bool RefreshDb()
        {
            return restService.RefreshDb();
        }

        public string GetLastChallonge()
        {
            return restService.GetLastChallonge();
        }
    }
}