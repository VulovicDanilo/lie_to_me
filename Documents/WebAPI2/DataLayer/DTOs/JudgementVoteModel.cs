using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DTOs
{
    public class JudgementVoteModel
    {
        public int gameId { get; set; }
        public int voterNumber { get; set; }
        public JudgementVote vote { get; set; }

        public JudgementVoteModel()
        {

        }
    }
}
