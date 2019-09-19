using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DTOs
{
    public class VotingVoteModel
    {
        public int gameId { get; set; }
        public int voterNumber { get; set; }
        public int votedNumber { get; set; }

        public VotingVoteModel()
        {

        }
    }
}
