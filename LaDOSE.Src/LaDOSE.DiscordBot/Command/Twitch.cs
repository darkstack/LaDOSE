using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace LaDOSE.DiscordBot.Command
{
    public class Twitch
    {
        private readonly Dependencies dep;

        public Twitch(Dependencies d)
        {
            dep = d;
        }

        [Command("twitch")]
        public async Task TwitchAsync(CommandContext ctx)
        {
            await ctx.RespondAsync("https://www.twitch.tv/LaDOSETV");
        }
    }
}