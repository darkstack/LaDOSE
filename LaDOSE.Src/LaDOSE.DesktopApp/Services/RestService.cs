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
        public List<WPEvent> GetEvents()
        {
            var restRequest = new RestRequest("/api/wordpress/WPEvent", Method.GET);
            var restResponse = Client.Get<List<WPEvent>>(restRequest);
            return restResponse.Data;
        }


        public string CreateChallonge(int gameId, int eventId)
        {
            var restRequest = new RestRequest($"/api/wordpress/CreateChallonge/{gameId}/{eventId}", Method.GET);
            var restResponse = Client.Get(restRequest);
            return restResponse.Content;
        }
        public string CreateChallonge2(int gameId, int eventId, List<WPUser> optionalPlayers)
        {
 
            var restResponse = Post<List<WPUser>,string>($"/api/wordpress/CreateChallonge/{gameId}/{eventId}",optionalPlayers);
            return restResponse;
        }
        public bool RefreshDb()
        {
            var restRequest = new RestRequest("/api/Wordpress/UpdateDb", Method.GET);
            var restResponse = Client.Get<bool>(restRequest);
            return restResponse.Data;
        }

        public List<WPUser> GetUsers(int wpEventId, int gameId)
        {
            var restRequest = new RestRequest($"/api/Wordpress/GetUsers/{wpEventId}/{gameId}", Method.GET);
            var restResponse = Client.Get<List<WPUser>>(restRequest);
            return restResponse.Data;
        }

        public List<WPUser> GetUsersOptions(int wpEventId, int gameId)
        {
            var restRequest = new RestRequest($"/api/Wordpress/GetUsersOptions/{wpEventId}/{gameId}", Method.GET);
            var restResponse = Client.Get<List<WPUser>>(restRequest);
            return restResponse.Data;
        }


        #endregion

        #region Games
        public List<Game> GetGames()
        {
            var restRequest = new RestRequest("/api/Game", Method.GET);
            var restResponse = Client.Get<List<Game>>(restRequest);
            return restResponse.Data;
        }

        public Game UpdateGame(Game eventUpdate)
        {
            return Post("Api/Game", eventUpdate);
        }
        #endregion

        #region Events


        #endregion





    }
}