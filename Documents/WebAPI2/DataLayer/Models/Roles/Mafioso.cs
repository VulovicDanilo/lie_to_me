using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class Mafioso : Role
    {
        public Mafioso()
            : base(RoleName.Mafioso, Alignment.Mafia, 5)
        {

        }

        public override void ExecuteAction(GameContext gameContext)
        {
            throw new NotImplementedException();
        }
    }
}