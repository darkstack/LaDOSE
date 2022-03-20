﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LaDOSE.Business.Provider.SmashProvider
{
    public class PageInfoType
    {
        public int total { get; set; }
        public int totalPages { get; set; }
        public int page { get; set; }
        public int perPage { get; set; }
        public string sortBy { get; set; }
        public string filter { get; set; }
    }                                       

    public class TournamentType
    {
        public int id { get; set; }

        public string Name { get; set; }

        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime startAt { get; set; }
        public List<EventType> Events { get; set; }

    }
    public class VideoGameType
    {
        public int id { get; set; }
        public string Name { get; set; }
    }

    public class StandingType
    {
        public int id { get; set; }

        public int placement { get; set; }

        public PlayerType player { get; set; }
    }
    public class PlayerType
    {
        public int id { get; set; }
        public string gamerTag { get; set; }
        public UserType user { get; set; }
    }
    public class UserType
    {
        public int id { get; set; }
        public string name { get; set; }

    }
    public class EventType
    {
        public int id { get; set; }

        public string name { get; set; }
        public string state { get; set; }

        public VideoGameType videogame { get; set; }
        public Node<StandingType> standings { get; set; }
    }

    public class Node<T>
    {
        public PageInfoType pageInfo { get; set; }
        public List<T> nodes { get; set; }

    }




    public class TournamentResponse
    {
        public TournamentType Tournament { get; set; }

    }

    public class EventResponse
    {
        public EventType Event { get; set; }

    }



}