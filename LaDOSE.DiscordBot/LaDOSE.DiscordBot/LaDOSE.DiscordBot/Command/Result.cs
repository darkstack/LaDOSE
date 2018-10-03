using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace LaDOSE.DiscordBot.Command
{
    public partial class Twitch
    {
        internal class Result
        {
            Dependencies dep;
            public Result(Dependencies d)
            {
                this.dep = d;
            }

            [Command("result")]
            public async Task ResultAsync(CommandContext ctx)
            {
                await ctx.RespondAsync("Resultat");
     
            }

            [Command("last")]
            public async Task LastAsync(CommandContext ctx)
            {
                var tournament = await dep.ChallongeService.GetLastTournament();
                await ctx.RespondAsync($"Dernier tournois: {tournament}");

            }
        }
    }
}