using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class LookoutStrategy : RoleStrategy
    {
        public LookoutStrategy()
            : base(RoleName.Lookout, Alignment.Town, 6, "watch one person at night to see who visits them", "defeat all mafia")
        {

        }

        public override void ExecuteAction(Game game)
        {
            throw new NotImplementedException();
        }
    }
}