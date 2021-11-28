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
        static  InteractivityModule Interactivity { get; set; }
        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("settings.json", optional: true, reloadOnChange: true).Build();

            var discordToken = builder["Discord:Token"].ToString();
            var challongeToken = builder["Challonge:Token"].ToString();
            var restUrl = builder["REST:Url"].ToString();
            var restUser = builder["REST:User"].ToString();
            var restPassword = builder["REST:Password"].ToString();

            Console.WriteLine($"LaDOSE.Net Discord Bot");


            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = discordToken,
                TokenType = TokenType.Bot
            });

            discord.UseInteractivity(new InteractivityConfiguration
            {
                // default pagination behaviour to just ignore the reactions
                PaginationBehaviour = TimeoutBehaviour.Ignore,

                // default pagination timeout to 5 minutes
                PaginationTimeout = TimeSpan.FromMinutes(5),

                // default timeout for other actions to 2 minutes
                Timeout = TimeSpan.FromMinutes(2)
            });
            var webService = new WebService(new Uri(restUrl),restUser,restPassword);
            //var challongeService = new ChallongeService(challongeToken);
            
            var cts = new CancellationTokenSource();
            DependencyCollection dep = null;

            using (var d = new DependencyCollectionBuilder())
            {
                d.AddInstance(new Dependencies()
                {
                    Cts = cts,
                    //ChallongeService = challongeService,
                    WebService = webService
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
            _cnext.RegisterCommands<Todo>();
            _cnext.RegisterCommands<Hokuto>();


            //discord.MessageCreated += async e =>
            //{
            //    if (e.Message.Content.ToLower().Equals("!result"))
            //        await e.Message.RespondAsync("Les Résultats du dernier Ranking : XXXX");
            //    if (e.Message.Content.ToLower().Equals("!twitch"))
            //        await e.Message.RespondAsync("https://www.twitch.tv/LaDOSETV");
            //};

            discord.GuildMemberAdded +=  async e =>
            {
                Console.WriteLine($"{e.Member.DisplayName} Joined");
                await Task.Delay(0);
                //await e.Guild.GetDefaultChannel().SendMessageAsync($"Bonjour!");
            };
            await discord.ConnectAsync();
            while (!cts.IsCancellationRequested)
            {
                await Task.Delay(200);
            }
            
        }
    }
}