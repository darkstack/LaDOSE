using System.Threading;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace LaDOSE.DiscordBot.Command
{
    [RequireRolesAttribute(RoleCheckMode.Any, "SuperAdmin")]
    public class Shutdown : BaseCommandModule
    {
        private readonly CancellationTokenSource cts;


        public Shutdown(CancellationTokenSource cts)
        {
            this.cts = cts;
        }

        [Command("shutdown")]
        public async Task ShutDownAsync(CommandContext ctx)
        {
            await ctx.RespondAsync("Hasta la vista, baby");
            cts.Cancel();
        }
    }
}