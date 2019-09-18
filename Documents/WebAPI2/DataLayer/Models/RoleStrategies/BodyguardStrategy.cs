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
            
        }

        public override void ExecuteAction(Game game)
        {
            throw new NotImplementedException();
        }
    }
}