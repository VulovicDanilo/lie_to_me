using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DTOs
{
    public class ExecuteActionModel
    {
        public int Who { get; set; }
        public int To { get; set; }
        public int gameId { get; set; }

        public ExecuteActionModel()
        {

        }

        public ExecuteActionModel(int who, int to)
        {
            Who = who;
            To = to;
        }
    }
}
