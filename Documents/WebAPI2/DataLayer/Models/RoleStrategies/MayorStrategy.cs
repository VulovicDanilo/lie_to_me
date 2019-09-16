using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class MayorStrategy : RoleStrategy
    {
        public MayorStrategy()
            : base(RoleName.Mayor, Alignment.Town, 10)
        {

        }

        public override void ExecuteAction(Game game)
        {
            throw new NotImplementedException();
        }

        public new static MayorStrategy CreateRoleStrategy() { return new MayorStrategy(); }
    }
}