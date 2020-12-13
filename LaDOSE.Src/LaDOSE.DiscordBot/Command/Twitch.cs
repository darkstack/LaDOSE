using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace LaDOSE.DiscordBot.Command
{
    public class Twitch : BaseCommandModule
    {

        [Command("twitch")]
        public async Task TwitchAsync(CommandContext ctx)
        {
            await ctx.RespondAsync("https://www.twitch.tv/LaDOSETV");
        }
    }
}