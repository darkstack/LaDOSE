using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using LaDOSE.DiscordBot.Service;
using LaDOSE.DTO;

namespace LaDOSE.DiscordBot.Command
{
    public class BotEvent : BaseCommandModule
    {
        private WebService dep;
        public BotEvent(WebService d)
        {
            dep = d;
        }

        [RequireRolesAttribute(RoleCheckMode.Any, "Staff")]
        [Command("newevent")]
        public async Task NewEventAsync(CommandContext ctx, string command)
        {

            await ctx.RespondAsync(dep.RestService.CreateBotEvent(command).ToString());
        }
        [RequireRolesAttribute(RoleCheckMode.Any,"Staff")]
        [Command("staffs")]
        public async Task StaffAsync(CommandContext ctx)
        {
            BotEventDTO currentEvent = dep.RestService.GetLastBotEvent();
            StringBuilder stringBuilder = new StringBuilder();

            var present = currentEvent.Results.Where(x => x.Result).ToList();
            var absent = currentEvent.Results.Where(x => !x.Result).ToList();

            stringBuilder.AppendLine($"Pour {currentEvent.Name} : ");
            present.ForEach(x => stringBuilder.AppendLine($":white_check_mark: {x.Name}"));
            absent.ForEach(x => stringBuilder.AppendLine($":x: {x.Name}"));

            await ctx.RespondAsync(stringBuilder.ToString());

        }
        [RequireRolesAttribute(RoleCheckMode.Any, "Staff")]
        [Command("present")]
        public async Task PresentAsync(CommandContext ctx)
        {
            await ctx.RespondAsync(dep.RestService.ResultBotEvent(new DTO.BotEventSendDTO() { DiscordId = ctx.Member.Id.ToString(), DiscordName = ctx.Member.DisplayName, Present = true }).ToString());


        }
        [RequireRolesAttribute(RoleCheckMode.Any, "Staff")]
        [Command("absent")]
        public async Task AbsentAsync(CommandContext ctx)
        {
            await ctx.RespondAsync(dep.RestService.ResultBotEvent(new DTO.BotEventSendDTO() { DiscordId = ctx.Member.Id.ToString(), DiscordName = ctx.Member.DisplayName, Present = false }).ToString());
        }
    }
}