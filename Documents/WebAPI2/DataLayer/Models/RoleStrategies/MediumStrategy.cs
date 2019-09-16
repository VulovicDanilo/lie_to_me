using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class MediumStrategy : RoleStrategy
    {
        public MediumStrategy() : base(RoleName.Medium, Alignment.Town, 10)
        {

        }
        public override void ExecuteAction(Game game)
        {
            
        }

        public new static MediumStrategy CreateRoleStrategy() { return new MediumStrategy(); }
    }
}