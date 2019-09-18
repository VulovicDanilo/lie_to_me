using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class MediumStrategy : RoleStrategy
    {
        public MediumStrategy() : base(RoleName.Medium, Alignment.Town, 10, "speak with all dead people at night", "defeat all mafia")
        {

        }
        public override void ExecuteAction(Game game)
        {
            
        }
    }
}