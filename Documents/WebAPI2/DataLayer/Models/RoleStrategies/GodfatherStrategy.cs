using DataLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class GodfatherStrategy : RoleStrategy
    {
        public GodfatherStrategy()
            : base(RoleName.Godfather, Alignment.Mafia, 5, "kill someone each night", "defeat all town")
        {
            this.Offence = Offence.Basic;
            this.Defence = Defence.Basic;
        }

        public override void ExecuteAction(Game game, ExecuteActionModel model)
        {
            var me = game.Players.Find(x => x.Number == model.Who);
            var toPlayer = game.Players.Find(x => x.Number == model.To);

            var mafioso = game.Players.Find(x => x.Role.RoleName == RoleName.Mafioso);

            if (mafioso != null)
            {
                toPlayer.Kill(mafioso);
            }
            else
            {
                if((int)toPlayer.Role.Defence < (int)Offence)
                    toPlayer.Kill(me);
            }
        }
    }
}