using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows;
using LaDOSE.DTO;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serialization.Json;
using DataFormat = RestSharp.DataFormat;

namespace LaDOSE.DesktopApp.Services
{
    public class RestService
    {
        public RestClient Client { get; set; }

        public RestService()
        {
#if DEBUG
            MessageBox.Show("WAIT");
#endif
            var appSettings = ConfigurationManager.AppSettings;
            string url = (string) appSettings["ApiUri"];
            string user = (string)appSettings["ApiUser"];
            string password = (string) appSettings["ApiPassword"];
            Client = new RestClient(url);
            var restRequest = new RestRequest("users/auth", Method.POST);
            restRequest.AddJsonBody(new { username = user, password = password });
            var response = Client.Post(restRequest);
            if (response.IsSuccessful)
            {
                JsonDeserializer d = new JsonDeserializer();
                var applicationUser = d.Deserialize<ApplicationUser>(response);
                Client.Authenticator = new JwtAuthenticator($"{applicationUser.Token}");
            }
            else
            {
                MessageBox.Show("Unable to contact services, i m useless, BYEKTHX","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                Application.Current.Shutdown(-1);
            }
        }

        #region PostFix

        private T Post<T>(string resource,T entity)
        {
            var json = new RestSharp.Serialization.Json.JsonSerializer();
            var jsonD = new RestSharp.Serialization.Json.JsonDeserializer();
            var request = new RestRequest();
            request.Method = Method.POST;
            request.Resource = resource;
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-type", "application/json");
            request.Parameters.Clear();
            request.AddParameter("application/json; charset=utf-8", json.Serialize(entity), ParameterType.RequestBody);
            request.AddObject(entity);
            var response = Client.Execute(request);
            //var content = response.Content; // raw content as string  
            try
            {
                return jsonD.Deserialize<T>(response);
            }
            catch (Exception)
            {
                return default(T);
            }


        }
        private R Post<P,R>(string resource, P entity)
        {
            var json = new RestSharp.Serialization.Json.JsonSerializer();
            var jsonD = new RestSharp.Serialization.Json.JsonDeserializer();
            var request = new RestRequest();
            request.Method = Method.POST;
            request.Resource = resource;
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-type", "application/json");
            request.Parameters.Clear();
            request.AddParameter("application/json; charset=utf-8", json.Serialize(entity), ParameterType.RequestBody);
            //request.AddObject(entity);
            var response = Client.Execute(request);
            //var content = response.Content; // raw content as string  
            try
            {
                return jsonD.Deserialize<R>(response);
            }
            catch (Exception)
            {
                return default(R);
            }


        }

        #endregion

        #region WordPress
        public List<WPEventDTO> GetEvents()
        {
            var restRequest = new RestRequest("/api/wordpress/WPEvent", Method.GET);
            var restResponse = Client.Get<List<WPEventDTO>>(restRequest);
            return restResponse.Data;
        }


        public string CreateChallonge(int gameId, int eventId)
        {
            var restRequest = new RestRequest($"/api/wordpress/CreateChallonge/{gameId}/{eventId}", Method.GET);
            var restResponse = Client.Get(restRequest);
            return restResponse.Content;
        }
        public string CreateChallonge2(int gameId, int eventId, List<WPUserDTO> optionalPlayers)
        {
 
            var restResponse = Post<List<WPUserDTO>,string>($"/api/wordpress/CreateChallonge/{gameId}/{eventId}",optionalPlayers);
            return restResponse;
        }
        public bool RefreshDb()
        {
            var restRequest = new RestRequest("/api/Wordpress/UpdateDb", Method.GET);
            var restResponse = Client.Get<bool>(restRequest);
            return restResponse.Data;
        }

        public List<WPUserDTO> GetUsers(int wpEventId, int gameId)
        {
            var restRequest = new RestRequest($"/api/Wordpress/GetUsers/{wpEventId}/{gameId}", Method.GET);
            var restResponse = Client.Get<List<WPUserDTO>>(restRequest);
            return restResponse.Data;
        }

        public List<WPUserDTO> GetUsersOptions(int wpEventId, int gameId)
        {
            var restRequest = new RestRequest($"/api/Wordpress/GetUsersOptions/{wpEventId}/{gameId}", Method.GET);
            var restResponse = Client.Get<List<WPUserDTO>>(restRequest);
            return restResponse.Data;
        }


        #endregion

        #region Games
        public List<GameDTO> GetGames()
        {
            var restRequest = new RestRequest("/api/Game", Method.GET);
            var restResponse = Client.Get<List<GameDTO>>(restRequest);
            return restResponse.Data;
        }

        public GameDTO UpdateGame(GameDTO game)
        {
            return Post("Api/Game", game);
        }
        public bool DeleteGame(int gameId)
        {
            var restRequest = new RestRequest($"/api/Game/{gameId}", Method.DELETE);
            var restResponse = Client.Execute(restRequest);
            return restResponse.IsSuccessful;
        }
        #endregion

        #region Events


        #endregion





    }
}