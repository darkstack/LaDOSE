using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Interactivity;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using LaDOSE.DiscordBot.Command;
using LaDOSE.DiscordBot.Service;
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
            var challongeToken = builder["Challonge:Token"].ToString();


            Console.WriteLine($"LaDOSE.Net Discord Bot");


            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = discordToken,
                TokenType = TokenType.Bot
            });


            var challongeService = new ChallongeService(challongeToken);
            var cts = new CancellationTokenSource();
            DependencyCollection dep = null;

            using (var d = new DependencyCollectionBuilder())
            {
                d.AddInstance(new Dependencies()
                {
                 
                    Cts = cts,
                    ChallongeService = challongeService
                });
                dep = d.Build();
            }

            var _cnext = discord.UseCommandsNext(new CommandsNextConfiguration()
            {
                CaseSensitive = false,
                EnableDefaultHelp = true,
                EnableDms = false,
                EnableMentionPrefix = true,
                StringPrefix = "!",
                IgnoreExtraArguments = true,
                Dependencies = dep
            });

            _cnext.RegisterCommands<Result>();
            _cnext.RegisterCommands<Twitch>();
            _cnext.RegisterCommands<Shutdown>();


            //discord.MessageCreated += async e =>
            //{
            //    if (e.Message.Content.ToLower().Equals("!result"))
            //        await e.Message.RespondAsync("Les Résultats du dernier Ranking : XXXX");
            //    if (e.Message.Content.ToLower().Equals("!twitch"))
            //        await e.Message.RespondAsync("https://www.twitch.tv/LaDOSETV");
            //};

            discord.GuildMemberAdded += async e =>
            {
                await e.Guild.GetDefaultChannel().SendMessageAsync($"Bonjour {e.Member.DisplayName}!");
            };
            await discord.ConnectAsync();
            while (!cts.IsCancellationRequested)
            {
                await Task.Delay(200);
            }
            
        }
    }
}