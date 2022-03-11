using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using LaDOSE.Business.Interface;
using LaDOSE.Entity.Challonge;

namespace LaDOSE.Business.Provider.SmashProvider
{
    public class SmashProvider : ISmashProvider
    {
        public string ApiKey { get; set; }
        public SmashProvider(string apiKey)
        {
            this.ApiKey = apiKey;
        }

        public Task<List<ChallongeTournament>> GetTournaments(DateTime? start, DateTime? end)
        {
            var list = new List<ChallongeTournament>();
            var personAndFilmsRequest = new GraphQLRequest
            {
                Query = @"
            query TournamentsByOwner($perPage: Int!, $ownerId: ID!) {
                tournaments(query: {
                    perPage: $perPage
                    filter: {
                        ownerId: $ownerId
                    }
                }) {
                    nodes {
                        id
                            name
                        slug
                    }
                }
            }",

                OperationName = "PersonAndFilms",
                Variables = new
                {

                    ownerId = "161429",
                    perPage = "4"

                }
            };


            return Task.FromResult(list);
        }

        public async Task<ResponseType> GetTournament(string slug)
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

            //GraphQLHttpRequest preprocessedRequest = await graphQLClient.Options.PreprocessRequest(Event, graphQLClient);
            //var x = preprocessedRequest.ToHttpRequestMessage(graphQLClient.Options, new NewtonsoftJsonSerializer());
            //System.Diagnostics.Trace.WriteLine(x.Content.ReadAsStringAsync().Result);
            //var sendAsync = await graphQLClient.HttpClient.SendAsync(x);
            //System.Diagnostics.Trace.WriteLine(sendAsync.Content.ReadAsStringAsync().Result);

            var graphQLResponse = await graphQLClient.SendQueryAsync<ResponseType>(Event);
            if (graphQLResponse.Errors != null)
            {
                //Event not done ? 
                //throw new Exception("Error");
            }
            System.Diagnostics.Trace.Write(graphQLResponse.Data.Tournament.Name);

            return graphQLResponse.Data;
        }
    }
}