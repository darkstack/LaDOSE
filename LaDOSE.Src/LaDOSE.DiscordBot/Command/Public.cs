using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace LaDOSE.DiscordBot.Command
{
    public class Public
    {
        private readonly Dependencies dep;
        private static List<string> Quotes { get; set; }
        private static List<string> Questions { get; set; }
        private static List<string> Answers { get; set; }
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


    
        [Command("roll")]
        public async Task RollAsync(CommandContext ctx, string command = "")
        {
            int next = 6;

            if (!string.IsNullOrEmpty(command))
            {
                if(!int.TryParse(command, System.Globalization.NumberStyles.Integer, CultureInfo.InvariantCulture, out next))
                {
                    await ctx.RespondAsync("Stupid");
                    return;
                }
            }
            await ctx.RespondAsync(rnd.Next(1,next+1).ToString());

        }


        #region Cards Against Humanity
        [Command("cards")]
        public async Task CardsAsync(CommandContext ctx)
        {
            if (Questions == null)
                LoadCards();
            var response = string.Empty;

            var q = Questions[rnd.Next(Questions.Count - 1)];

            var s = q.Split('_', StringSplitOptions.None);
            if (s.Length == 1)
            {
                response += q + " " + Answers[rnd.Next(Answers.Count - 1)];
            }
            else
            {
                for (int i = 0; i < s.Length - 1; i++)
                {
                    response += s[i] + Answers[rnd.Next(Answers.Count - 1)];
                }
                response += s[s.Length - 1];
            }
            await ctx.RespondAsync(response);
        }

        private void LoadCards()
        {
            try
            {
                Questions = File.ReadAllLines("questions.txt").ToList();
                Answers = File.ReadAllLines("answers.txt").ToList();
            }
            catch (FileNotFoundException)
            {
                Questions = new List<string>();
                Answers = new List<string>();
            }

        }
        #endregion
        #region Quotes
        [Command("Quote")]
        public async Task QuotesAsync(CommandContext ctx)
        {
            if (Quotes == null)
                LoadQuote();

            await ctx.RespondAsync("```" + Quotes[rnd.Next(Quotes.Count - 1)] + "```");
        }



        private void LoadQuote()
        {
            Quotes = new List<string>();
            string[] fileQuotes = File.ReadAllLines("quotes.txt");
            string currentQuote = string.Empty;
            for (int i = 0; i < fileQuotes.Length; i++)
            {
                if (String.IsNullOrEmpty(fileQuotes[i]))
                {
                    Quotes.Add(currentQuote);
                    currentQuote = string.Empty;
                }

                currentQuote += fileQuotes[i] + (!String.IsNullOrEmpty(currentQuote) ? "\r\n" : "");
            }

        }
        #endregion  
    }
}