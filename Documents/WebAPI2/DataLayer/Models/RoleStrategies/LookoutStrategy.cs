using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class LookoutStrategy : RoleStrategy
    {
        public LookoutStrategy()
            : base(RoleName.Lookout, Alignment.Town, 6)
        {

        }

        public override void ExecuteAction(Game game)
        {
            throw new NotImplementedException();
        }

        public new static LookoutStrategy CreateRoleStrategy() { return new LookoutStrategy(); }
    }
}