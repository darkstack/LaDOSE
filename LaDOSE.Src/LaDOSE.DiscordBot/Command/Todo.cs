using System;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity;
using LaDOSE.DTO;

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
        public async Task TodoAsync(CommandContext ctx, string command,params string[] todo)
        {
            await ctx.TriggerTypingAsync();
            string args = string.Join(" ",todo);
            switch (command.ToUpperInvariant())
            {
                case "ADD":
                   
                    var todoDto = new TodoDTO
                    {
                        Created = DateTime.Now,
                        Done = false,
                        Deleted = null,
                        Task = args,
                        User = ctx.User.Username,
                    };
                    dep.WebService.RestService.UpdateTodo(todoDto);
                    //dep.WebService.RestService.UpdateGame();
                    break;
                case "LIST":
                    var todoDtos = dep.WebService.RestService.GetTodos();
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Todos: ");
                    if (todoDtos!=null && todoDtos.Count>0)
                    { 
                        foreach (var task in todoDtos)
                        {
                            string taskStatus = task.Done ? ":white_check_mark:" : ":negative_squared_cross_mark:";
                            sb.AppendLine($"{task.Id} | {taskStatus} | {task.User} | {task.Task}");
                        }
                    }
                    else
                    {
                        sb.AppendLine("None.");
                    }
                    await ctx.RespondAsync(sb.ToString());
                    break;
                case "DEL":
                    try
                    {
                        int id = int.Parse(todo[0]);
                        await ctx.RespondAsync(dep.WebService.RestService.DeleteTodo(id) ? $"Deleted" : $"Error");
                    }
                    catch (Exception e)
                    {
                        await ctx.RespondAsync($"Error {e.Message}");
                        return;
                    }
                    break;
                case "V":
                    try
                    {
                        int id = int.Parse(todo[0]);
                        var todoById = dep.WebService.RestService.GetTodoById(id);
                        todoById.Done = true;
                        dep.WebService.RestService.UpdateTodo(todoById);
                        await ctx.RespondAsync($"Done : {todoById.Id} - {todoById.Task}");
                    }
                    catch (Exception e)
                    {
                        await ctx.RespondAsync($"Error {e.Message}");
                        return;
                    }
                    break;
                case "X":
                    try
                    {
                        int id = int.Parse(todo[0]);
                        var todoById = dep.WebService.RestService.GetTodoById(id);
                        todoById.Done = false;
                        dep.WebService.RestService.UpdateTodo(todoById);
                        await ctx.RespondAsync($"Undone : {todoById.Id} - {todoById.Task}");
                    }
                    catch (Exception e)
                    {
                        await ctx.RespondAsync($"Erreur {e.Message}");
                        return;
                    }
                    break;
                case "TRUNC":
                    try
                    {
          
                        var todos = dep.WebService.RestService.GetTodos();
                        await ctx.RespondAsync($"Sure ? (Y/N)");
                        var interactivity = ctx.Client.GetInteractivityModule();
                        var waitForMessageAsync = await interactivity.WaitForMessageAsync(xm => xm.Content.Contains("Y")||xm.Content.Contains("N"), TimeSpan.FromSeconds(10));
                        if (waitForMessageAsync!= null)
                        {
                            if (waitForMessageAsync.Message.Content == "Y")
                            {
                                foreach (var task in todos)
                                {
                                    dep.WebService.RestService.DeleteTodo(task.Id);
                                    await ctx.RespondAsync($"Deleted - {task.Id}");
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        await ctx.RespondAsync($"Erreur {e.Message}");
                        return;
                    }
                    break;

            }
            //await ctx.RespondAsync($"command : {command}, todo:  {todo} ");
        }
    }
}