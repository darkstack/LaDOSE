using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace LaDOSE.DiscordBot.Command
{

        internal class Result
        {
            Dependencies dep;
            public Result(Dependencies d)
            {
                this.dep = d;
            }


            [RequireRolesAttribute("Staff")]
            [Command("update")]
            public async Task UpdateAsync(CommandContext ctx)
            {
                var tournament = await dep.ChallongeService.GetLastTournament();
                await ctx.RespondAsync($"Mise à jour effectuée");

            }

            [Command("last")]
            public async Task LastAsync(CommandContext ctx)
            {
                var lastTournamentMessage = dep.ChallongeService.GetLastTournamentMessage();
                await ctx.RespondAsync(lastTournamentMessage);

            }
        }
    
}