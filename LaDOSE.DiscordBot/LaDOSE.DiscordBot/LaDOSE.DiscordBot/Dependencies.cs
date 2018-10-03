using System.Threading;
using DSharpPlus.Interactivity;

namespace LaDOSE.DiscordBot
{
    internal class Dependencies
    {
        internal InteractivityModule Interactivity { get; set; }
        internal CancellationTokenSource Cts { get; set; }
    }
}