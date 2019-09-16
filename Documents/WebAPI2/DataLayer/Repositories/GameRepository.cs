using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class GameRepository : Repository<Game>
    {
        public GameRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
