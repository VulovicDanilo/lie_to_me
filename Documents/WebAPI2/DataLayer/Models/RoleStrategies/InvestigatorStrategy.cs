using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class InvestigatorStrategy :RoleStrategy
    {
        public InvestigatorStrategy()
            :base(RoleName.Investigator, Alignment.Town, 4)
        {

        }

        public override void ExecuteAction(Game game)
        {
            throw new NotImplementedException();
        }
    }
}