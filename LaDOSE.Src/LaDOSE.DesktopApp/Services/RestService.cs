using System;
using System.Collections.Generic;
using System.Windows;
using LaDOSE.DTO;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serialization.Json;

namespace LaDOSE.DesktopApp.Services
{
    public class RestService
    {
        public RestClient Client { get; set; }

        public RestService()
        {
            Client = new RestClient("http://localhost:5000/");
            var restRequest = new RestRequest("users/auth", Method.POST);
            restRequest.AddJsonBody(new { username = "****", password = "*****" });
            var response = Client.Post(restRequest);
            if (response.IsSuccessful)
            {
                JsonDeserializer d = new JsonDeserializer();
                var applicationUser = d.Deserialize<ApplicationUser>(response);
                Client.Authenticator = new JwtAuthenticator($"{applicationUser.Token}");
            }
            else
            {
                throw new Exception("No Service Avaliable");
            }
        }

        public List<WPEvent> GetEvents()
        {
            var restRequest = new RestRequest("/api/wordpress/WPEvent", Method.GET);
            var restResponse = Client.Get<List<WPEvent>>(restRequest);
            return restResponse.Data;
        }

        public List<Game> GetGames()
        {
            var restRequest = new RestRequest("/api/Game", Method.GET);
            var restResponse = Client.Get<List<Game>>(restRequest);
            return restResponse.Data;
        }
    }
}