using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using LaDOSE.Business.Interface;
using LaDOSE.Business.Service;
using LaDOSE.Entity;
using LaDOSE.Entity.Challonge;
using Result = LaDOSE.Entity.Result;

namespace LaDOSE.Business.Provider.SmashProvider
{
    public class SmashProvider : ISmashProvider
    {
        public string ApiKey { get; set; }
        //public SmashProvider(string apiKey)
        //{
        //    this.ApiKey = apiKey;
        //}
        public SmashProvider(IGameService gameService, IEventService eventService, IPlayerService playerService, string apiKey)
        {
            this.ApiKey = apiKey;
            this.GameService = gameService;
            this.EventService = eventService;
            this.PlayerService = playerService;
        }

        public IPlayerService PlayerService { get; set; }
        public IEventService EventService { get; set; }
        public IGameService GameService { get; set; }

        private async Task<T> QuerySmash<T>(GraphQLRequest req)
        {
            var graphQLClient = new GraphQLHttpClient("https://api.smash.gg/gql/alpha", new NewtonsoftJsonSerializer());
            graphQLClient.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");

            var graphQLResponse = await graphQLClient.SendQueryAsync<T>(req);
            if (graphQLResponse.Errors != null)
            {
                //EventType not done ? 
                //throw new Exception("Error");
            }

            return graphQLResponse.Data;
        }

        public async Task<Event> GetEvent(string slug)
        {

            var query = new GraphQLRequest
            {
                Query = @"query TournamentQuery($slug: String) {
                                  tournament(slug: $slug){
                                    id
                                    name,
                                    startAt,
                                    events {
                                      id
                                      name,
                                      state,
                                      videogame {
                                        id,
                                        name,
                                        displayName
                                        }
                                      }
                                    }
                            }",
                OperationName = "TournamentQuery",
                Variables = new
                {
                    slug = slug,
                }
            };
            var games = GameService.GetAll();
            var querySmash = QuerySmash<TournamentResponse>(query);
            List<Tournament> tournaments = querySmash.Result.Tournament.Events.Select(e => new Tournament()
            {
                Name = e.name,
                SmashId = e.id,
                Finish = e.state == "COMPLETED",
                Game = games.FirstOrDefault(g => g.SmashId == e.videogame.id)
                
            }).ToList();

            return new Event
            {
                SmashSlug = slug,
                Name = querySmash.Result.Tournament.Name,
                SmashId = querySmash.Result.Tournament.id,
                Date = querySmash.Result.Tournament.startAt,
                Tournaments = tournaments,

            };

        }

        public Task<List<Tournament>> GetResults(ref List<Tournament> tournaments)
        {

            foreach (var tournament in tournaments.Where(t=>t.Finish).ToList())
            {
                var query = new GraphQLRequest
                {
                    Query = @"query EventQuery($event: ID,$page:Int) {
                          event(id: $event){
                             standings(query: {page:$page,perPage:20}){
      						                    pageInfo {
      						                      total
      						                      totalPages
      						                      page
      						                      perPage
      						                      sortBy
      						                      filter
      						                    },
                                      nodes{
                                        id,
                                        player{
                                          id,
                                          gamerTag,
                                          user {
                                            id,
                                            name,
          			                            player {
          			                              id
          			                            }
                                          }
                                        }
                                        placement
                                      }
                            }
                            }
                    }",
                    OperationName = "EventQuery",
                    Variables = new
                    {
                        page = 1,
                        @event = tournament.SmashId,
                    }
                };
                var querySmash = QuerySmash<EventResponse>(query);

                if (querySmash.Result.Event.standings.nodes != null)
                {
                    var standings = querySmash.Result.Event.standings.nodes;
                    if (querySmash.Result.Event.standings.pageInfo.totalPages > 1)
                    {
                        while (querySmash.Result.Event.standings.pageInfo.page <
                               querySmash.Result.Event.standings.pageInfo.totalPages)
                        {
                            query.Variables = new
                            {
                                page = querySmash.Result.Event.standings.pageInfo.page+1,
                                @event = tournament.SmashId,
                            };
                            querySmash = QuerySmash<EventResponse>(query);
                            standings.AddRange(querySmash.Result.Event.standings.nodes);
                        }
                    }

                    var res= standings.Select(x => new Result()
                    {
                        Tournament = tournament,
                        TournamentId = tournament.Id,
                    
                        PlayerId = PlayerService.GetIdBySmash(x.player),
                        Rank = x.placement
                    }).ToList();
                    tournament.Results = res;
                }

                //tournament.Results.AddRange();
                //List<Tournament> tournaments = querySmash.Result.Tournament.Events.Select(e => new Tournament()
                //{
                //    Name = e.name,
                //    SmashId = e.id,
                //    Game = games.FirstOrDefault(g => g.SmashId == e.videogame.id)
                //}).ToList();



            }

            return Task.FromResult(tournaments);
        }

        public Task<List<Tournament>> GetSets(ref List<Tournament> tournaments)
        {

            foreach (var tournament in tournaments.Where(t => t.Finish).ToList())
            {
                var query = new GraphQLRequest
                {
                    Query = @"query SetsQuery($event: ID,$page:Int) {
                    event(id: $event){
                      sets(page:$page,perPage:20){
                        pageInfo {
                          total
                          totalPages
                          page
                          perPage
                          sortBy
                          filter
                        },
                        nodes {
                          id,
                          lPlacement,
                          wPlacement, 
                          round,    
                          slots {
                            id,
                            slotIndex,
                            standing{
                              id,
                              player{
                                id,
                                user{
                                  id,
                                }
                              }
                              stats{
                                score {
                                  label
                                  value
                                  displayValue
                                }
                              }
                              
                            }
                            entrant {
                              id,
                              name,
                              participants{
                                id,
                                gamerTag
                                user {
                                  id
                                },
                              }
                            },
                          },
                          phaseGroup {
                            id
                          },
                          identifier
                        },

                      }
                    }
                    }",
                    OperationName = "SetsQuery",
                    Variables = new
                    {
                        page = 1,
                        @event = tournament.SmashId,
                    }
                };
                var querySmash = QuerySmash<SetsResponse>(query);

                if (querySmash.Result.Event.sets.nodes != null)
                {
                    var sets = querySmash.Result.Event.sets.nodes;
                    if (querySmash.Result.Event.sets.pageInfo.totalPages > 1)
                    {
                        while (querySmash.Result.Event.sets.pageInfo.page <
                               querySmash.Result.Event.sets.pageInfo.totalPages)
                        {
                            query.Variables = new
                            {
                                page = querySmash.Result.Event.sets.pageInfo.page+1,
                                @event = tournament.SmashId,
                            };
                            querySmash = QuerySmash<SetsResponse>(query);
                            sets.AddRange(querySmash.Result.Event.sets.nodes);
                        }
                    }

                    var tset = sets.Select(x => new Set()
                    {
                        Tournament = tournament,
                        TournamentId = tournament.Id,

                        Player1Id = PlayerService.GetIdBySmash(x.slots[0].entrant.participants[0]),
                        Player2Id = PlayerService.GetIdBySmash(x.slots[1].entrant.participants[0]),
                        Player1Score = x.slots[0].standing.stats.score.value.HasValue ? x.slots[0].standing.stats.score.value.Value : 0,
                        Player2Score = x.slots[1].standing.stats.score.value.HasValue ? x.slots[1].standing.stats.score.value.Value : 0,
                        Round = x.round ?? 0,
                    }).ToList();
                    tournament.Sets = tset;
                }
            }
            return Task.FromResult(tournaments);

        }

        public async Task<Event> ParseEvent(string slug)
        {
            Event e = await this.GetEvent(slug);
            return e;
        }


        public async Task<TournamentResponse> GetTournament(string slug)
        {

            var graphQLClient = new GraphQLHttpClient("https://api.smash.gg/gql/alpha", new NewtonsoftJsonSerializer());
            graphQLClient.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");
            var Event = new GraphQLRequest
            {
                Query = @"query TournamentQuery($slug: String) {
		tournament(slug: $slug){
			id
			name
			events {
				id
				name,
                state,
                    videogame {
                      id,
                      name,
                      displayName
                    }
                standings(query: {page:0,perPage:500}){
                  nodes{
                    id,
                    player{
                      id,
                      gamerTag,
                      user {
                        id,
                        name,
          			        player {
          			          id
          			        }
                      }
                    }
                    placement
          }
        
        }
        }
			
		}
	}",
                OperationName = "TournamentQuery",
                Variables = new
                {
                    slug = slug,
                }
            };

            //GraphQLHttpRequest preprocessedRequest = await graphQLClient.Options.PreprocessRequest(EventType, graphQLClient);
            //var x = preprocessedRequest.ToHttpRequestMessage(graphQLClient.Options, new NewtonsoftJsonSerializer());
            //System.Diagnostics.Trace.WriteLine(x.Content.ReadAsStringAsync().Result);
            //var sendAsync = await graphQLClient.HttpClient.SendAsync(x);
            //System.Diagnostics.Trace.WriteLine(sendAsync.Content.ReadAsStringAsync().Result);

            var graphQLResponse = await graphQLClient.SendQueryAsync<TournamentResponse>(Event);
            if (graphQLResponse.Errors != null)
            {
                //EventType not done ? 
                //throw new Exception("Error");
            }
            System.Diagnostics.Trace.Write(graphQLResponse.Data.Tournament.Name);

            return graphQLResponse.Data;
        }
    }
}