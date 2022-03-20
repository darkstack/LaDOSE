using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LaDOSE.Entity
{
    public class Game : Context.Entity
    {
        public string Name { get; set; }
        public string LongName { get; set; }
        public string ImgUrl { get; set; }
        public int  Order { get; set; }
        public string WordPressTag { get; set; }
        public string WordPressTagOs { get; set; }

        public int? SmashId { get; set; }


        public List<Tournament> Tournaments { get; set; }

    }

}
