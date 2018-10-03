using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace LaDOSE.DiscordBot.Command
{
    public partial class Result
    {

        internal class Twitch
        {
            Dependencies dep;
            public Twitch(Dependencies d)
            {
                this.dep = d;
            }

            [Command("twitch")]
            public async Task TwitchAsync(CommandContext ctx)
            {
                await ctx.RespondAsync("https://www.twitch.tv/LaDOSETV");
     
            }
        }
    }
}