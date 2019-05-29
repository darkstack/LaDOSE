using System.Collections.Generic;

namespace LaDOSE.Entity.Challonge
{
    public class Tournament
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Game { get; set; }
        public List<Participent> Participents { get; set; }
    }
}