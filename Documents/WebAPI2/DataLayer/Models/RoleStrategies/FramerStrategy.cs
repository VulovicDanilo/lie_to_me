using DataLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class FramerStrategy : RoleStrategy
    {
        public FramerStrategy()
            : base(RoleName.Framer, Alignment.Mafia, 2, "choose one person to frame each night", "defeat all town")
        {
            this.Offence = Offence.None;
            this.Defence = Defence.None;
        }

        public override void ExecuteAction(Game game, ExecuteActionModel model)
        {
            var toPlayer = game.Players.Find(x => x.Number == model.To);

            toPlayer.Suspicious = true;
        }
    }
}