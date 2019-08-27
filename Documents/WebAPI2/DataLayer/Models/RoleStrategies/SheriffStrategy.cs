using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class SheriffStrategy : RoleStrategy
    {
        public SheriffStrategy() : base(RoleName.Sheriff, Alignment.Town, 4)
        {

        }
        public override void ExecuteAction(Game game)
        {
            throw new NotImplementedException();
        }
    }
}