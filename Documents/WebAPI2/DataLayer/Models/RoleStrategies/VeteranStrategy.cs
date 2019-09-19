using DataLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class VeteranStrategy : RoleStrategy
    {
        private int NumberOfVisits { get; set; } = 3;
        public VeteranStrategy() : base(RoleName.Veteran, Alignment.Town, 1, "decide if you will go on alert and kill anyone who visits you", "defeat all mafia")
        {
            this.Offence = Offence.None;
            this.Defence = Defence.None;
            this.CanVisit = false;
        }
        public override void ExecuteAction(Game game, ExecuteActionModel model)
        {
            if (NumberOfVisits != 0)
            {
                NumberOfVisits--;

                this.Offence = Offence.Powerful;
                this.Defence = Defence.Basic;

                var player = game.Players.Find(x => x.Number == model.Who);

                foreach (var visitor in player.Visitors)
                {
                    visitor.Kill(player);
                }
                player.Done = true;
            }
        }
    }
}