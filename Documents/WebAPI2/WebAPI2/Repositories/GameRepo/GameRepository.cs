using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPI2.Models;

namespace WebAPI2.Repositories.GameRepo
{
    /// <summary>
    /// Specilized Game Repository
    /// </summary>
    public class GameRepository:Repository<Game>,IGameRepository
    {
        public GameRepository(ApplicationDbContext context)
            :base(context)
        {

        }
        public List<Player> AlivePlayers
        {
            get
            {
                return Context.Players.Where(x => x.Alive).ToList();
            }
        }

        public List<Player> DeadPlayers
        {
            get
            {
                return Context.Players.Where(x => x.Alive == false).ToList();
            }
        }
    }
}