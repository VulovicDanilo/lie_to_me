using DataLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class BodyguardStrategy : RoleStrategy
    {
        public BodyguardStrategy() : base(RoleName.BodyGuard, Alignment.Town, 3, "protect one person from death each night", "defeat all mafia")
        {
            this.Offence = Offence.Powerful;
            this.Defence = Defence.None;
        }

        public override void ExecuteAction(Game game, ExecuteActionModel model)
        {

        }
    }
}