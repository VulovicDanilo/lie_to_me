using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI2.Models;

namespace WebAPI2.Repositories.GameRepo
{
    public interface IGameRepository
    {
        List<Player> AlivePlayers { get; }

        List<Player> DeadPlayers { get; }
    }
}
