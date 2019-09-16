using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class GodfatherStrategy : RoleStrategy
    {
        public GodfatherStrategy()
            : base(RoleName.Godfather, Alignment.Mafia, 5)
        {

        }

        public override void ExecuteAction(Game game)
        {
            throw new NotImplementedException();
        }

        public new static GodfatherStrategy CreateRoleStrategy() { return new GodfatherStrategy(); }
    }
}