using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class SerialKillerStrategy : RoleStrategy
    {
        public SerialKillerStrategy() : base(RoleName.SerialKiller, Alignment.Neutral, 5, "kill someone each night", "be last man standing")
        {

        }
        public override void ExecuteAction(Game game)
        {
            throw new NotImplementedException();
        }
    }
}