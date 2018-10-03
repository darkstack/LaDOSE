using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Interactivity;
using Microsoft.Extensions.Configuration;

namespace LaDOSE.DiscordBot
{
    class Program
    {

        static DiscordClient discord;
        static InteractivityModule _interactivity;
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

            var _interactivity = discord.UseInteractivity(new InteractivityConfiguration()
            {
                PaginationBehaviour = TimeoutBehaviour.Delete,
                PaginationTimeout = TimeSpan.FromSeconds(30),
                Timeout = TimeSpan.FromSeconds(30)
            });

            discord.MessageCreated += async e =>
            {
                if (e.Message.Content.ToLower().Equals("!result"))
                    await e.Message.RespondAsync("Les Résultats du dernier Ranking : XXXX");
                if (e.Message.Content.ToLower().Equals("!twitch"))
                    await e.Message.RespondAsync("https://www.twitch.tv/LaDOSETV");
            };

            discord.GuildMemberAdded += async e =>
            {
                await e.Guild.GetDefaultChannel().SendMessageAsync($"Bonjour {e.Member.Nickname}!");
            };
            await discord.ConnectAsync();
            await Task.Delay(-1);
        }
    
    }
    
}




