using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace LaDOSE.DiscordBot.Command
{
    [RequireRolesAttribute("SuperAdmin")]
    public class Shutdown
    {
        private readonly Dependencies dep;

        public Shutdown(Dependencies d)
        {
            dep = d;
        }


        [Command("shutdown")]
        public async Task ShutDownAsync(CommandContext ctx)
        {
            await ctx.RespondAsync("Hasta la vista, baby");
            dep.Cts.Cancel();
        }
    }
}