using DataLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class SerialKillerStrategy : RoleStrategy
    {
        public SerialKillerStrategy() : base(RoleName.SerialKiller, Alignment.Neutral, 5, "kill someone each night", "be last man standing")
        {
            this.Offence = Offence.Basic;
            this.Defence = Defence.Basic;
        }
        public override void ExecuteAction(Game game, ExecuteActionModel model)
        {
            var me = game.Players.Find(x => x.Number == model.Who);
            var toPlayer = game.Players.Find(x => x.Number == model.To);
            if((int)toPlayer.Role.Defence < (int)Offence)
                toPlayer.Kill(me);

        }
    }
}