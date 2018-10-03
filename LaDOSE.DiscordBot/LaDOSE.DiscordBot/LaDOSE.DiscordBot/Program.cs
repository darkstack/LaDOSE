using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DSharpPlus;
using Microsoft.Extensions.Configuration;

namespace LaDOSE.DiscordBot
{
    class Program
    {

        static DiscordClient discord;
        
        static void Main(string[] args)
        {
           

            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("settings.json", optional: true, reloadOnChange: true).Build();

            var discordToken = builder["Discord:Token"].ToString();


            Console.WriteLine($"LaDOSE.Net Discord Bot");

            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = discordToken,
                TokenType = TokenType.Bot
            });

            discord.MessageCreated += async e =>
            {
                if (e.Message.Content.ToLower().Equals("!result"))
                    await e.Message.RespondAsync("Les Résultats du dernier Ranking : XXXX");
            };

            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    
    }
    
}




