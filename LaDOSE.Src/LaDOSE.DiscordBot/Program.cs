using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Interactivity;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity.Extensions;
//using DSharpPlus.SlashCommands;
//using DSharpPlus.SlashCommands.Attributes;
using LaDOSE.DiscordBot.Command;
using LaDOSE.DiscordBot.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LaDOSE.DiscordBot
{
    class Program
    {
        static DiscordClient discord;

        //static  InteractivityModule Interactivity { get; set; }
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

            var service = new ServiceCollection()
                .AddSingleton(typeof(WebService), new WebService(new Uri(restUrl), restUser, restPassword))
                .BuildServiceProvider();


            Console.WriteLine($"LaDOSE.Net Discord Bot");

            
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = discordToken,
                TokenType = TokenType.Bot,
                //AutoReconnect = true,
                //MinimumLogLevel = LogLevel.Debug,
                //MessageCacheSize = 0,
            });

            discord.UseInteractivity(new InteractivityConfiguration
            {

                // default pagination behaviour to just ignore the reactions
                //PaginationBehaviour = TimeoutBehaviour.Ignore,

                // default pagination timeout to 5 minutes
                //PaginationTimeout = TimeSpan.FromMinutes(5),

                // default timeout for other actions to 2 minutes
                Timeout = TimeSpan.FromMinutes(2)
            });


            var cts = new CancellationTokenSource();

            var _cnext = discord.UseCommandsNext(new CommandsNextConfiguration()
            {
                //CaseSensitive = false,
                //EnableDefaultHelp = true,
                //EnableDms = false,
                //EnableMentionPrefix = true,
                StringPrefixes = new List<string>() { "/", "!" },
                //IgnoreExtraArguments = true,
                Services = service
            });


            //var slashCommands =  discord.UseSlashCommands(new SlashCommandsConfiguration() {Services = service});
            //slashCommands.RegisterCommands<SlashCommand>(guildId:null);


            //_cnext.RegisterCommands<Result>();
            _cnext.RegisterCommands<Public>();
            //_cnext.RegisterCommands<Shutdown>();
            //_cnext.RegisterCommands<Todo>();
            _cnext.RegisterCommands<Hokuto>();
            _cnext.RegisterCommands<BotEvent>();

            foreach (var registeredCommandsKey in discord.GetCommandsNext().RegisteredCommands.Keys)
            {
                Console.WriteLine(registeredCommandsKey);
            }


            discord.Ready += (sender, eventArgs) =>
            {
                Console.WriteLine($"Bot READY.");
                return Task.CompletedTask;
            };
            discord.GuildAvailable += (sender, eventArgs) =>
            {

                Console.WriteLine($"Joined Guild " + eventArgs.Guild.Name);
                return Task.CompletedTask;
            };

            await discord.ConnectAsync();
            

            await Task.Delay(Timeout.Infinite);

            //while (!cts.IsCancellationRequested)
            //{
            //    await Task.Delay(200);
            //    //if(discord.GetConnectionsAsync().Result.Count)
            //}
            
        }
    

    }

    //internal class SlashCommand : ApplicationCommandModule
    //{
    //    [SlashCommand("test", "A slash command made to test the DSharpPlusSlashCommands library!")]
        
    //    public async Task TestCommand(InteractionContext ctx)
    //    {

    //        await ctx.CreateResponseAsync("Lol");
            
    //    }
    //}

}