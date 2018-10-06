using System.Collections.Generic;
using LaDOSE.Entity;

namespace LaDOSE.Business.Interface
{
    public interface IGameService
    {
   
        IEnumerable<Game> GetAll();
        Game GetById(int id);
        Game Create(Game game);
        bool Update(Game game);
        void Delete(int id);
    }
}