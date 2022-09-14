using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace LaDOSE.DiscordBot.Command
{

    public class Hokuto : BaseCommandModule
    {
        
        private static List<string> Games = new List<string> { "2X", "3.3", "Karnov" };
        private static Random r = new Random();
        public Hokuto()
        {
            
        }


        [Command("hokuto")]
        public async Task HokutoUserAsync(CommandContext ctx, params DiscordMember[] user)
        {


            var i = r.Next(0, 3);
            if (user!=null && user.Length>0)
            {
                await ctx.RespondAsync(ctx.User?.Mention + " vs " + user[0].Mention + " : " + Games[i].ToString());
            }
            else
            {
                await ctx.RespondAsync(ctx.User?.Mention + " : " + Games[i].ToString());
            }

        }
    }
}