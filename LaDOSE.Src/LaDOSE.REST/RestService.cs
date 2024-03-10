using System;
using System.Collections.Generic;
using LaDOSE.DTO;
using LaDOSE.REST.Event;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;


namespace LaDOSE.REST
{
    public class RestService
    {
        private string username { get; set; }
        private string password { get; set; }

        private ApplicationUserDTO Auth { get; set; }

        public string UserName => Auth?.FirstName;

        public DateTime ValidUntil => Auth.Expire;
        //private ApplicationUserDTO token = null;
        public RestClient Client { get; set; }

        public event EventHandler<UpdatedJwtEventHandler> UpdatedJwtEvent;

        public RestService()
        {
            
        }
    

        public void Connect(Uri url, string user, string password)
        {
            Client = new RestClient(url);
            string token = GetToken(user, password);
            Client = new RestClient(url, options =>
            {
#if DEBUG
                options.MaxTimeout = Int32.MaxValue;
#endif
                options.Authenticator = new JwtAuthenticator(token);
            });

            this.username = user;
            this.password = password;
            
        }

        private string GetToken(string user, string password)
        {
            var restRequest = new RestRequest("users/auth", Method.Post);
            restRequest.AddJsonBody(new {username = user, password = password});
            
            var response = Client.Post<ApplicationUserDTO>(restRequest);
            //var applicationUser = JsonConvert.DeserializeObject<ApplicationUserDTO>(response.Content);
            this.Auth = response;
            return response.Token;
        }

        private void RaiseUpdatedJwtEvent(UpdatedJwtEventHandler auth)
        {
            EventHandler<UpdatedJwtEventHandler> handler = UpdatedJwtEvent;
            
            handler?.Invoke(this, auth);

        }


        private void CheckToken()
        {
            if (this.Auth == null || this.Auth.Expire <= DateTime.Now)
            {
                GetToken(this.username,this.password);
            }
        }

// #region PostFix
//
//         private T Post<T>(string resource,T entity)
//         {
//             var json = new RestSharp.Serialization.Json.JsonSerializer();
//             var jsonD = new RestSharp.Serialization.Json.JsonDeserializer();
//             var request = new RestRequest();
//             request.Method = Method.Post;
//             request.Resource = resource;
//             request.AddHeader("Accept", "application/json");
//             request.AddHeader("Content-type", "application/json");
//             request.Parameters.Clear();
//             request.AddParameter("application/json; charset=utf-8", json.Serialize(entity), ParameterType.RequestBody);
//             request.AddObject(entity);
//             var response = Client.Execute(request);
//             //var content = response.Content; // raw content as string  
//             try
//             {
//                 return jsonD.Deserialize<T>(response);
//             }
//             catch (Exception)
//             {
//                 return default(T);
//             }
//
//
//         }
//         private R Post<P,R>(string resource, P entity)
//         {
//             var json = new RestSharp.Serialization.Json.JsonSerializer();
//             var jsonD = new RestSharp.Serialization.Json.JsonDeserializer();
//             var request = new RestRequest();
//             request.Method = Method.Post;
//             request.Resource = resource;
//             request.AddHeader("Accept", "application/json");
//             request.AddHeader("Content-type", "application/json");
//             request.Parameters.Clear();
//             request.AddParameter("application/json; charset=utf-8", json.Serialize(entity), ParameterType.RequestBody);
//             //request.AddObject(entity);
//             var response = Client.Execute(request);
//             //var content = response.Content; // raw content as string  
//             try
//             {
//                 return jsonD.Deserialize<R>(response);
//             }
//             catch (Exception)
//             {
//                 return default(R);
//             }
//
//
//         }
//
// #endregion

#region WordPress
        public List<WPEventDTO> GetEvents()
        {
            CheckToken();
            var restRequest = new RestRequest("/api/wordpress/WPEvent", Method.Get);
            var restResponse = Client.Get<List<WPEventDTO>>(restRequest);
            return restResponse;
        }
        public WPEventDTO GetNextEvent()
        {
            CheckToken();
            var restRequest = new RestRequest("/api/wordpress/NextEvent", Method.Get);
            var restResponse = Client.Get<WPEventDTO>(restRequest);
            return restResponse;
        }


        public string GetLastChallonge()
        {
            CheckToken();
            var restRequest = new RestRequest($"/api/wordpress/GetLastChallonge/", Method.Get);
            var restResponse = Client.Get(restRequest);
            return restResponse.Content;
        }
        public string CreateChallonge(int gameId, int eventId)
        {
            CheckToken();
            var restRequest = new RestRequest($"/api/wordpress/CreateChallonge/{gameId}/{eventId}", Method.Get);
            var restResponse = Client.Get(restRequest);
            return restResponse.Content;
        }
        public string CreateChallonge2(int gameId, int eventId, List<WPUserDTO> optionalPlayers)
        {
            CheckToken();
            RestRequest r =
                new RestRequest($"/api/wordpress/CreateChallonge/{gameId}/{eventId}").AddJsonBody(optionalPlayers);
            var restResponse = Client.Post<string>(r);
            return restResponse;
        }
        public bool RefreshDb()
        {
            CheckToken();
            var restRequest = new RestRequest("/api/Wordpress/UpdateDb", Method.Get);
            var restResponse = Client.Get<bool>(restRequest);
            return restResponse;
        }

        public List<WPUserDTO> GetUsers(int wpEventId, int gameId)
        {
            CheckToken();
            var restRequest = new RestRequest($"/api/Wordpress/GetUsers/{wpEventId}/{gameId}", Method.Get);
            var restResponse = Client.Get<List<WPUserDTO>>(restRequest);
            return restResponse;
        }

        public List<WPUserDTO> GetUsersOptions(int wpEventId, int gameId)
        {
            CheckToken();
            var restRequest = new RestRequest($"/api/Wordpress/GetUsersOptions/{wpEventId}/{gameId}", Method.Get);
            var restResponse = Client.Get<List<WPUserDTO>>(restRequest);
            return restResponse;
        }


#endregion

#region Games
        public List<GameDTO> GetGames()
        {
            CheckToken();
            var restRequest = new RestRequest("/api/Game", Method.Get);
            var restResponse = Client.Get<List<GameDTO>>(restRequest);
            return restResponse;
        }

        public GameDTO UpdateGame(GameDTO game)
        {
            CheckToken();
            RestRequest r = new RestRequest("Api/Game").AddJsonBody(game);
            return Client.Post<GameDTO>(r);
        }
        public bool DeleteGame(int gameId)
        {
            CheckToken();
            var restRequest = new RestRequest($"/api/Game/{gameId}", Method.Delete);
            var restResponse = Client.Execute(restRequest);
            return restResponse.IsSuccessful;
        }
#endregion

#region Events


#endregion

#region Todo

        public List<TodoDTO> GetTodos()
        {
            CheckToken();
            var restRequest = new RestRequest("/api/Todo", Method.Get);
            var restResponse = Client.Get<List<TodoDTO>>(restRequest);
            return restResponse;
        }
        public TodoDTO GetTodoById(int id)
        {
            CheckToken();
            var restRequest = new RestRequest($"/api/Todo/{id}", Method.Get);
            var restResponse = Client.Get<TodoDTO>(restRequest);
            return restResponse;
        }
        public TodoDTO UpdateTodo(TodoDTO Todo)
        {
            CheckToken();
            var restRequest = new RestRequest($"/api/Todo/", Method.Post).AddJsonBody(Todo);
            return Client.Post<TodoDTO>(restRequest);
        }
        public bool DeleteTodo(int todoId)
        {
            CheckToken();
            var restRequest = new RestRequest($"/api/Todo/{todoId}", Method.Delete);
            var restResponse = Client.Execute(restRequest);
            return restResponse.IsSuccessful;
        }


#endregion
#region Tournaments

        public TournamentsResultDTO Test(string test)
        {
            CheckToken();
            var restRequest = new RestRequest($"Api/Test/Test/{test}", Method.Get);
            var restResponse = Client.Get<TournamentsResultDTO>(restRequest);
            return restResponse;
            
        }

#endregion
#region Tournaments

        public List<TournamentDTO> GetTournaments(TimeRangeDTO timeRange)
        {
            CheckToken();
            RestRequest r = new RestRequest("/api/Tournament/GetTournaments").AddJsonBody(timeRange);
            List<TournamentDTO> tournamentDtos = Client.Post<List<TournamentDTO>>(r);
            return tournamentDtos;
        }

        public TournamentsResultDTO GetResults(List<int> ids)
        {
            
            CheckToken();
            var restRequest = new RestRequest("Api/Tournament/GetResults", Method.Post).AddJsonBody(ids);
            return Client.Post<TournamentsResultDTO>(restRequest);
            
        }


        public bool ParseSmash(string slug)
        {
            CheckToken();
            var restRequest = new RestRequest($"Api/Tournament/ParseSmash/{slug}", Method.Get);
            var restResponse = Client.Get<bool>(restRequest);
            return restResponse;
         
        }

        public bool ParseChallonge(List<int> ids)
        {
            CheckToken();
            var restRequest = new RestRequest("Api/Tournament/ParseChallonge", Method.Post).AddJsonBody(ids);
            return Client.Post<bool>(restRequest);
        }
#endregion
#region Tournamenet Event / Player
        public List<EventDTO> GetAllEvents()
        {
            CheckToken();
            var restRequest = new RestRequest("/api/Event", Method.Get);
            var restResponse = Client.Get<List<EventDTO>>(restRequest);
            return restResponse;
        }

        public List<string> GetPlayers(string slug)
        {
            CheckToken();
            var restRequest = new RestRequest($"/api/Tournament/GetPLayers/{slug}", Method.Get);
            var restResponse = Client.Get<List<string>>(restRequest);
            return restResponse;
        }
#endregion


#region Bot Command

        public bool CreateBotEvent(string eventName)
        {
            CheckToken();
            var restRequest = new RestRequest($"/api/BotEvent/CreateBotEvent/{eventName}", Method.Get);
            var restResponse = Client.Get<bool>(restRequest);
            return restResponse;
        }

        public BotEventDTO GetLastBotEvent()
        {
            CheckToken();
            var restRequest = new RestRequest($"/api/BotEvent/GetLastBotEvent/", Method.Get);
            var restResponse = Client.Post<BotEventDTO>(restRequest);
            return restResponse;
        }

        public bool ResultBotEvent(BotEventSendDTO result)
        {
            CheckToken();
            var restRequest = new RestRequest("/api/BotEvent/ResultBotEvent", Method.Post).AddJsonBody(result);
            return Client.Post<bool>(restRequest);
        }

#endregion
    }
}