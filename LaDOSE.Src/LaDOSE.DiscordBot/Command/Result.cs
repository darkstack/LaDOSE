using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using LaDOSE.DiscordBot.Service;

namespace LaDOSE.DiscordBot.Command
{
    public class Result : BaseCommandModule
    {
        WebService dep;


        public Result(WebService d)
        {
            this.dep = d;
        }


        //[RequireRolesAttribute("Staff")]
        //[Command("update")]
        //public async Task UpdateAsync(CommandContext ctx)
        //{
        //    //var tournament = await dep.ChallongeService.GetLastTournament();
        //    //await ctx.RespondAsync($"Mise à jour effectuée");
        //}



        [Command("last")]
        public async Task LastAsync(CommandContext ctx)
        {
            var lastTournamentMessage = dep.GetLastChallonge();
            await ctx.RespondAsync(lastTournamentMessage);
        }

        [RequireRolesAttribute(RoleCheckMode.Any,"Staff")]
        [Command("inscriptions")]
        public async Task InscriptionsAsync(CommandContext ctx)
        {
            await ctx.TriggerTypingAsync();
            var inscrits = dep.GetInscrits();
            await ctx.RespondAsync(inscrits);
        }

        [RequireRolesAttribute(RoleCheckMode.Any, "Staff")]
        [Command("UpdateDb")]
        public async Task UpdateDbAsync(CommandContext ctx)
        {
            await ctx.RespondAsync("Mise à jour des inscriptions en cours...");
            await ctx.TriggerTypingAsync();

            var status = dep.RefreshDb() ? "Ok" : "erreur";

            await ctx.RespondAsync($"Status: {status}");
        }
    }
}