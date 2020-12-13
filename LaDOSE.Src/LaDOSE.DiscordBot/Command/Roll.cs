using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace LaDOSE.DiscordBot.Command
{
    public class Roll : BaseCommandModule
    {
        public List<String> jeux = new List<string>() { "Karnov", "2X", "Third Strike" };
        public static Random Random = new Random(unchecked((int)DateTime.Now.Ticks));
        public static bool romok = false;
        public String romurl = "Not yet ! ";

        [RequireRolesAttribute(RoleCheckMode.Any, "Staff")]
        [Command("setrom")]
        public async Task SetRoms(CommandContext ctx, string command, params string[] url)
        {
            string args = string.Join(" ", url);
            switch (command.ToUpperInvariant())
            {
                case "SETURL":
                    romurl = args;
                    break;
                case "LOCK":
                    romok = true;
                    break;
                case "UNLOCK":
                    romok = false;
                    break;

            }

            await ctx.TriggerTypingAsync();
            await ctx.RespondAsync("");
        }


        [Command("rom")]
        public async Task Roms(CommandContext ctx, string command, params string[] todo)
        {
            if (romok)
            {
                await ctx.TriggerTypingAsync();
                await ctx.RespondAsync(romurl);
                return;
            }
            
          
            await ctx.RespondAsync("");
        }
        [Command("hokuto")]
        public async Task hokuto2(CommandContext ctx)
        {
            List<DiscordMember> discordMembers = new List<DiscordMember>();
            

            if (ctx.Message.MentionedUsers.Count==1 && ctx.Message.MentionedUsers[0] != null)
                await ctx.RespondAsync(ctx.Member.Mention + " vs " + ctx.Message.MentionedUsers.First().Mention + " : " + jeux[Random.Next(0, 3)]);
            else
                await ctx.RespondAsync(ctx.Member.Mention + " " + jeux[Random.Next(0, 3)]);
        }

    }
}