using System.Threading;
using DSharpPlus.Interactivity;
using LaDOSE.DiscordBot.Service;

namespace LaDOSE.DiscordBot
{
    internal class Dependencies
    {
        internal InteractivityModule Interactivity { get; set; }
        internal CancellationTokenSource Cts { get; set; }
        public ChallongeService ChallongeService { get; set; }
    }
}