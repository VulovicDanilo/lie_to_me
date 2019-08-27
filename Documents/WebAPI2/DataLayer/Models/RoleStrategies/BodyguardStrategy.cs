using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class BodyguardStrategy : RoleStrategy
    {
        public BodyguardStrategy() : base(RoleName.BodyGuard, Alignment.Town, 3)
        {
            
        }

        public override void ExecuteAction(Game game)
        {
            throw new NotImplementedException();
        }


    }
}