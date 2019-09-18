using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class MafiosoStrategy : RoleStrategy
    {
        public MafiosoStrategy()
            : base(RoleName.Mafioso, Alignment.Mafia, 5, "carry out the Godfather's orders", "defeat all town")
        {

        }

        public override void ExecuteAction(Game game)
        {
            throw new NotImplementedException();
        }
    }
}