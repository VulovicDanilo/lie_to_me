using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class PlayerRepository : Repository<Player>
    {
        public PlayerRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void UpdatePlayers(IEnumerable<Player> players)
        {
            foreach (var player in players)
            {
                this.Update(player);
            }

        }
    }
}
