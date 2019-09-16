using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class JesterStrategy : RoleStrategy
    {
        public JesterStrategy()
            : base(RoleName.Jester, Alignment.Neutral, 10)
        {

        }

        public override void ExecuteAction(Game game)
        {
            throw new NotImplementedException();
        }
    }
}