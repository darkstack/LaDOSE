using LaDOSE.Entity.BotEvent;

namespace LaDOSE.Business.Interface
{
    public interface IBotEventService : IBaseService<BotEvent>
    {
        BotEvent GetLastEvent();

        bool CreateEvent(string EventName);
        bool SetResult(string discordId, string name, bool present);
    }
}