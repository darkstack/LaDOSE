using System.Collections.Generic;
using LaDOSE.Entity;

namespace LaDOSE.Business.Interface
{
    public interface IGameService : IBaseService<Game>
    {
        public int? GetIdByName(string name);

    }
}