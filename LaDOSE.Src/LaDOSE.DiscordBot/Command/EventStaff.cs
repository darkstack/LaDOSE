using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using LaDOSE.DTO;

namespace LaDOSE.DiscordBot.Command
{
    public class BotEvent
    {
        private readonly Dependencies dep;

        public BotEvent(Dependencies d)
        {
            dep = d;
        }

        [RequireRolesAttribute("Staff")]
        [Command("newevent")]
        public async Task NewEventAsync(CommandContext ctx, string command)
        {

            await ctx.RespondAsync(dep.WebService.RestService.CreateBotEvent(command).ToString());
        }
        [RequireRolesAttribute("Staff")]
        [Command("staffs")]
        public async Task StaffAsync(CommandContext ctx)
        {
            BotEventDTO currentEvent = dep.WebService.RestService.GetLastBotEvent();
            StringBuilder stringBuilder = new StringBuilder();

            var present = currentEvent.Results.Where(x => x.Result).ToList();
            var absent = currentEvent.Results.Where(x => !x.Result).ToList();

            stringBuilder.AppendLine($"Pour {currentEvent.Name} : ");
            present.ForEach(x => stringBuilder.AppendLine($":white_check_mark: {x.Name}"));
            absent.ForEach(x => stringBuilder.AppendLine($":x: {x.Name}"));

            await ctx.RespondAsync(stringBuilder.ToString());

        }
        [RequireRolesAttribute("Staff")]
        [Command("present")]
        public async Task PresentAsync(CommandContext ctx)
        {
            await ctx.RespondAsync(dep.WebService.RestService.ResultBotEvent(new DTO.BotEventSendDTO() { DiscordId = ctx.Member.Id.ToString(), DiscordName = ctx.Member.DisplayName, Present = true }).ToString());


        }
        [RequireRolesAttribute("Staff")]
        [Command("absent")]
        public async Task AbsentAsync(CommandContext ctx)
        {
            await ctx.RespondAsync(dep.WebService.RestService.ResultBotEvent(new DTO.BotEventSendDTO() { DiscordId = ctx.Member.Id.ToString(), DiscordName = ctx.Member.DisplayName, Present = false }).ToString());
        }
    }
}