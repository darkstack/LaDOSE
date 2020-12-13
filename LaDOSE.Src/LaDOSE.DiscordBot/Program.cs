using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Interactivity;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using LaDOSE.DiscordBot.Command;
using LaDOSE.DiscordBot.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LaDOSE.DiscordBot
{
    class Program
    {
        static DiscordClient discord;
        static  InteractivityConfiguration Interactivity { get; set; }
        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
#if DEBUG
            //Wait in debug mode
            Thread.Sleep(5*1000);
#endif
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
                PaginationBehaviour = PaginationBehaviour.Ignore,

                //// default pagination timeout to 5 minutes
                //PaginationTimeout = TimeSpan.FromMinutes(5),

                // default timeout for other actions to 2 minutes
                Timeout = TimeSpan.FromMinutes(2)
            });

            var depco = new ServiceCollection();
            var webService = new WebService(new Uri(restUrl),restUser,restPassword);
            depco.AddSingleton(webService);
            //var challongeService = new ChallongeService(challongeToken);
            


            var cts = new CancellationTokenSource();
            depco.AddSingleton(cts);



            var _cnext = discord.UseCommandsNext(new CommandsNextConfiguration()
            {
                CaseSensitive = false,
                EnableDefaultHelp = true,
                EnableDms = false,
                EnableMentionPrefix = true,
                StringPrefixes = new List<string>(){"!"},
                IgnoreExtraArguments = true,
                Services = depco.BuildServiceProvider(),

            });

            
            _cnext.RegisterCommands<Result>();
            _cnext.RegisterCommands<Twitch>();
            _cnext.RegisterCommands<Shutdown>();
            //_cnext.RegisterCommands<Todo>();
            _cnext.RegisterCommands<Roll>();

            discord.GuildMemberAdded += async (s, e) => 
            {
                Console.WriteLine($"{e.Member.DisplayName} Joined");
                //await Task.Delay(0);
            };
            await discord.ConnectAsync();
            while (!cts.IsCancellationRequested)
            {
                await Task.Delay(200);
            }
            
        }
    }
}