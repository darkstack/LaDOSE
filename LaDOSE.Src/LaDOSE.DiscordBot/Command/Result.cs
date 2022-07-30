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


        //[Command("last")]
        //public async Task LastAsync(CommandContext ctx)
        //{
        //    var lastTournamentMessage = dep.WebService.GetLastChallonge();
        //    await ctx.RespondAsync(lastTournamentMessage);
        //}

        //[RequireRolesAttribute("Staff")]
        //[Command("inscriptions")]
        //public async Task InscriptionsAsync(CommandContext ctx)
        //{
        //    await ctx.TriggerTypingAsync();
        //    var inscrits = dep.WebService.GetInscrits();
        //    await ctx.RespondAsync(inscrits);
        //}

        [RequireRolesAttribute("Staff")]
        [Command("UpdateDb")]
        public async Task UpdateDbAsync(CommandContext ctx)
        {
            await ctx.RespondAsync("Mise à jour des inscriptions en cours...");
            await ctx.TriggerTypingAsync();

            var status = dep.WebService.RefreshDb() ? "Ok" : "erreur";

            await ctx.RespondAsync($"Status: {status}");
        }
    }
}