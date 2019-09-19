using DataLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class VigilanteStrategy : RoleStrategy
    {
        public VigilanteStrategy() : base(RoleName.Vigilante, Alignment.Town, 5, "choose to take justice into your own hands and shoot someone", "defeat all mafia")
        {
            this.Offence = Offence.Basic;
            this.Defence = Defence.None;
        }

        public override void ExecuteAction(Game game, ExecuteActionModel model)
        {
            var me = game.Players.Find(x => x.Number == model.Who);
            var toPlayer = game.Players.Find(x => x.Number == model.To);

            if((int)toPlayer.Role.Defence < (int)Offence)
            {
                toPlayer.Kill(me);
            }

            me.Done = true;

        }
    }
}