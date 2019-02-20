using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace LaDOSE.DiscordBot.Command
{
    public class Todo
    {
        private readonly Dependencies dep;

        public Todo(Dependencies d)
        {
            dep = d;
        }

        [Command("todo")]
        public async Task TwitchAsync(CommandContext ctx, string command,params string[] todo)
        {
            await ctx.TriggerTypingAsync();
            switch (command.ToUpperInvariant())
            {
                case "ADD":
                    dep.TodoService.Add(todo[0]);
                    break;
                case "LIST":
                    await ctx.RespondAsync($"{dep.TodoService.List()}");
                    break;
                case "DEL":
                    int id;
                    if (int.TryParse(todo[0], out id))
                    {
                        await ctx.RespondAsync($"{dep.TodoService.Delete(id)}");
                        break;
                    };
                    await ctx.RespondAsync($"invalid id");
                    break;
            }
            //await ctx.RespondAsync($"command : {command}, todo:  {todo} ");
        }
    }
}