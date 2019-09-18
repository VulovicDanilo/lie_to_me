using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class VeteranStrategy : RoleStrategy
    {
        public VeteranStrategy() : base(RoleName.Veteran, Alignment.Town, 1, "decide if you will go on alert and kill anyone who visits you", "defeat all mafia")
        {

        }
        public override void ExecuteAction(Game game)
        {
            throw new NotImplementedException();
        }
    }
}