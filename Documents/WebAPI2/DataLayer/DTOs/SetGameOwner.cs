using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DTOs
{
    public class SetGameOwner
    {
        public int gameId { get; set; }
        public int playerId { get; set; }
    }
}
