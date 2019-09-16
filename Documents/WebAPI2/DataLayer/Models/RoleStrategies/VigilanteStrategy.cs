using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class VigilanteStrategy : RoleStrategy
    {
        public VigilanteStrategy() : base(RoleName.Vigilante, Alignment.Town, 5)
        {
        }

        public override void ExecuteAction(Game game)
        {
            throw new NotImplementedException();
        }
    }
}