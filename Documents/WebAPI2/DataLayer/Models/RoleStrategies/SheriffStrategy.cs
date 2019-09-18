using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class SheriffStrategy : RoleStrategy
    {
        public SheriffStrategy() : base(RoleName.Sheriff, Alignment.Town, 4, "check one person each night for suspicious activity", "defeat all mafia")
        {

        }
        public override void ExecuteAction(Game game)
        {
            throw new NotImplementedException();
        }
    }
}