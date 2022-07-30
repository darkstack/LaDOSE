using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace LaDOSE.DiscordBot.Command
{
    public class Public
    {
        private readonly Dependencies dep;
        private List<string> Quotes { get; set; }
        private Random rnd { get; set; }
        public Public(Dependencies d)
        {
            dep = d;
            rnd = new Random(DateTime.Now.Millisecond);
        }

        [Command("twitch")]
        public async Task TwitchAsync(CommandContext ctx)
        {
            await ctx.RespondAsync("https://www.twitch.tv/LaDOSETV");
        }

        [Command("Quote")]
        public async Task QuotesAsync(CommandContext ctx)
        {

            if (Quotes == null)
                LoadQuote();

            await ctx.RespondAsync("```"+ Quotes[rnd.Next(Quotes.Count-1)] + "```");
        }

        private void LoadQuote()
        {
            Quotes = new List<string>();
            string[] fileQuotes = File.ReadAllLines("quotes.txt");
            string currentQuote = string.Empty;
            for(int i = 0; i < fileQuotes.Length; i++)
            {
                if(String.IsNullOrEmpty(fileQuotes[i]))
                {
                    Quotes.Add(currentQuote);
                    currentQuote = string.Empty;
                }

                currentQuote += fileQuotes[i] + (!String.IsNullOrEmpty(currentQuote) ? "\r\n" : "");
            }
            
        }
    }
}