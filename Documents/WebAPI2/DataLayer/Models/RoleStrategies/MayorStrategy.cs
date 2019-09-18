using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class MayorStrategy : RoleStrategy
    {
        public MayorStrategy()
            : base(RoleName.Mayor, Alignment.Town, 10, "gain 3 votes when you reveal yourself as mayor", "defeat all mafia")
        {

        }

        public override void ExecuteAction(Game game)
        {
            throw new NotImplementedException();
        }
    }
}