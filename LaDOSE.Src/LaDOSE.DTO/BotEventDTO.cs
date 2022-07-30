using System;
using System.Collections.Generic;

namespace LaDOSE.DTO
{
    public class BotEventSendDTO
    {
        public string DiscordId { get; set; }
        public string DiscordName { get; set; }
        public bool Present { get; set; }
    }
    public class BotEventDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public List<BotEventResultDTO> Results { get; set; }

    }
}

