using System;
using System.Collections.Generic;
using System.Linq;
using LaDOSE.Business.Interface;
using LaDOSE.Entity;
using LaDOSE.Entity.Context;
using Microsoft.EntityFrameworkCore;

namespace LaDOSE.Business.Service
{
    public class GameService : IGameService
    {
        private LaDOSEDbContext _context;

        public GameService(LaDOSEDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Game> GetAll()
        {
            return _context.Game.Include(e => e.Seasons).ToList();
        }

        public Game GetById(int id)
        {
            return _context.Game.Include(e => e.Seasons).FirstOrDefault(e=>e.Id == id);
        }

        public Game Create(Game game)
        {
            if (game.Id != 0)
            {
                throw new Exception("Id is invalid");
            }
            var gameAdded = _context.Game.Add(game);
            _context.SaveChanges();
            return gameAdded.Entity;
        }

        public bool Update(Game game)
        {
            if (game.Id == 0)
            {
                throw new Exception("Id is invalid");
            }

            var gameUpdated = _context.Game.FirstOrDefault(e => e == game);
            
            gameUpdated = game;
            try
            {
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            return false;


        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}