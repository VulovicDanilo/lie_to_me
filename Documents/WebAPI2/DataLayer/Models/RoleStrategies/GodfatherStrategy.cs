using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class GodfatherStrategy : RoleStrategy
    {
        public GodfatherStrategy()
            : base(RoleName.Godfather, Alignment.Mafia, 5, "kill someone each night", "defeat all town")
        {

        }

        public override void ExecuteAction(Game game)
        {
            throw new NotImplementedException();
        }
    }
}