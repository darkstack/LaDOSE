using System.Collections.Generic;

namespace LaDOSE.Business.Provider.SmashProvider
{
    public class ResponseType
    {
        public TournamentType Tournament { get; set; }
        public class TournamentType
        {
            public int id { get; set; }

            public string Name { get; set; }

            public List<Event> Events { get; set; }

        }

        public class Event
        {
            public int id { get; set; }

            public string name { get; set; }
            public string state { get; set; }
            
            public VideoGame videogame { get; set; }
            public Node<Standing> standings { get; set; }
        }

        public class VideoGame
        {
            public int id { get; set; }
            public string Name { get; set; }
        }

        public class Node<T>
        {
            public List<T> nodes { get; set; }
           
        }
        public class Standing
        {
            public int id { get; set; }
 
            public int placement { get; set; }

            public Player player { get; set; }
        }
        public class Player
        {
            public int id { get; set; }
            public string gamerTag { get; set; }
            public User user { get; set; }
        }
        public class User
        {
            public int id { get; set; }
            public string name { get; set; }
      
        }

    }




}