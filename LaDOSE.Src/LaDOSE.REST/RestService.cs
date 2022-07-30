using System;
using System.Collections.Generic;
using LaDOSE.DTO;
using LaDOSE.REST.Event;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Serialization.Json;

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

        public RestService() { }
    

        public void Connect(Uri url, string user, string password)
        {
            Client = new RestClient(url);
#if DEBUG
            Client.Timeout = 999*1000;
#endif
            this.username = user;
            this.password = password;
            GetToken(user, password);
        }

        private void GetToken(string user, string password)
        {
            var restRequest = new RestRequest("users/auth", Method.POST);
            restRequest.AddJsonBody(new {username = user, password = password});

            var response = Client.Post(restRequest);
            if (response.IsSuccessful)
            {
                JsonDeserializer d = new JsonDeserializer();
                var applicationUser = d.Deserialize<ApplicationUserDTO>(response);
                this.Auth = applicationUser;
                Client.Authenticator = new JwtAuthenticator($"{applicationUser.Token}");
                RaiseUpdatedJwtEvent(new UpdatedJwtEventHandler(this.Auth));
            }
            else
            {

                throw new Exception("unable to contact services");
            }
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
            CheckToken();
            var restRequest = new RestRequest("/api/wordpress/WPEvent", Method.GET);
            var restResponse = Client.Get<List<WPEventDTO>>(restRequest);
            return restResponse.Data;
        }
        public WPEventDTO GetNextEvent()
        {
            CheckToken();
            var restRequest = new RestRequest("/api/wordpress/NextEvent", Method.GET);
            var restResponse = Client.Get<WPEventDTO>(restRequest);
            return restResponse.Data;
        }


        public string GetLastChallonge()
        {
            CheckToken();
            var restRequest = new RestRequest($"/api/wordpress/GetLastChallonge/", Method.GET);
            var restResponse = Client.Get(restRequest);
            return restResponse.Content;
        }
        public string CreateChallonge(int gameId, int eventId)
        {
            CheckToken();
            var restRequest = new RestRequest($"/api/wordpress/CreateChallonge/{gameId}/{eventId}", Method.GET);
            var restResponse = Client.Get(restRequest);
            return restResponse.Content;
        }
        public string CreateChallonge2(int gameId, int eventId, List<WPUserDTO> optionalPlayers)
        {
            CheckToken();
            var restResponse = Post<List<WPUserDTO>,string>($"/api/wordpress/CreateChallonge/{gameId}/{eventId}",optionalPlayers);
            return restResponse;
        }
        public bool RefreshDb()
        {
            CheckToken();
            var restRequest = new RestRequest("/api/Wordpress/UpdateDb", Method.GET);
            var restResponse = Client.Get<bool>(restRequest);
            return restResponse.Data;
        }

        public List<WPUserDTO> GetUsers(int wpEventId, int gameId)
        {
            CheckToken();
            var restRequest = new RestRequest($"/api/Wordpress/GetUsers/{wpEventId}/{gameId}", Method.GET);
            var restResponse = Client.Get<List<WPUserDTO>>(restRequest);
            return restResponse.Data;
        }

        public List<WPUserDTO> GetUsersOptions(int wpEventId, int gameId)
        {
            CheckToken();
            var restRequest = new RestRequest($"/api/Wordpress/GetUsersOptions/{wpEventId}/{gameId}", Method.GET);
            var restResponse = Client.Get<List<WPUserDTO>>(restRequest);
            return restResponse.Data;
        }


#endregion

#region Games
        public List<GameDTO> GetGames()
        {
            CheckToken();
            var restRequest = new RestRequest("/api/Game", Method.GET);
            var restResponse = Client.Get<List<GameDTO>>(restRequest);
            return restResponse.Data;
        }

        public GameDTO UpdateGame(GameDTO game)
        {
            CheckToken();
            return Post("Api/Game", game);
        }
        public bool DeleteGame(int gameId)
        {
            CheckToken();
            var restRequest = new RestRequest($"/api/Game/{gameId}", Method.DELETE);
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
            var restRequest = new RestRequest("/api/Todo", Method.GET);
            var restResponse = Client.Get<List<TodoDTO>>(restRequest);
            return restResponse.Data;
        }
        public TodoDTO GetTodoById(int id)
        {
            CheckToken();
            var restRequest = new RestRequest($"/api/Todo/{id}", Method.GET);
            var restResponse = Client.Get<TodoDTO>(restRequest);
            return restResponse.Data;
        }
        public TodoDTO UpdateTodo(TodoDTO Todo)
        {
            CheckToken();
            return Post("Api/Todo", Todo);
        }
        public bool DeleteTodo(int todoId)
        {
            CheckToken();
            var restRequest = new RestRequest($"/api/Todo/{todoId}", Method.DELETE);
            var restResponse = Client.Execute(restRequest);
            return restResponse.IsSuccessful;
        }


#endregion
#region Tournaments

        public TournamentsResultDTO Test(string test)
        {
            CheckToken();
            var restRequest = new RestRequest($"Api/Test/Test/{test}", Method.GET);
            var restResponse = Client.Get<TournamentsResultDTO>(restRequest);
            return restResponse.Data;
            
        }

#endregion
#region Tournaments

        public List<TournamentDTO> GetTournaments(TimeRangeDTO timeRange)
        {
            CheckToken();
            List<TournamentDTO> tournamentDtos = Post<TimeRangeDTO, List<TournamentDTO>>("/api/Tournament/GetTournaments",timeRange);
            return tournamentDtos;
        }

        public TournamentsResultDTO GetResults(List<int> ids)
        {
            CheckToken();
            return Post<List<int>,TournamentsResultDTO>("Api/Tournament/GetResults", ids);
            
        }


        public bool ParseSmash(string slug)
        {
            CheckToken();
            var restRequest = new RestRequest($"Api/Tournament/ParseSmash/{slug}", Method.GET);
            var restResponse = Client.Get<bool>(restRequest);
            return restResponse.Data;
         
        }

        public bool ParseChallonge(List<int> ids)
        {
            CheckToken();
            return Post<List<int>, bool>("Api/Tournament/ParseChallonge", ids);
        }
#endregion
#region Tournamenet Event / Player
        public List<EventDTO> GetAllEvents()
        {
            CheckToken();
            var restRequest = new RestRequest("/api/Event", Method.GET);
            var restResponse = Client.Get<List<EventDTO>>(restRequest);
            return restResponse.Data;
        }

        public List<string> GetPlayers(string slug)
        {
            CheckToken();
            var restRequest = new RestRequest($"/api/Tournament/GetPLayers/{slug}", Method.GET);
            var restResponse = Client.Get<List<string>>(restRequest);
            return restResponse.Data;
        }
#endregion


#region Bot Command

        public bool CreateBotEvent(string eventName)
        {
            CheckToken();
            var restRequest = new RestRequest($"/api/BotEvent/CreateBotEvent/{eventName}", Method.GET);
            var restResponse = Client.Get<bool>(restRequest);
            return restResponse.Data;
        }

        public BotEventDTO GetLastBotEvent()
        {
            CheckToken();
            var restRequest = new RestRequest($"/api/BotEvent/GetLastBotEvent/", Method.GET);
            var restResponse = Client.Post<BotEventDTO>(restRequest);
            return restResponse.Data;
        }

        public bool ResultBotEvent(BotEventSendDTO result)
        {
            CheckToken();
            return Post<BotEventSendDTO,bool>("/api/BotEvent/ResultBotEvent", result);
        }

#endregion
    }
}