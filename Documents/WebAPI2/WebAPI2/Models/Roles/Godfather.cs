using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI2.Models.Roles
{
    public class Godfather : Role
    {
        public Godfather()
            : base(RoleName.Godfather, Alignment.Mafia, 5)
        {

        }

        public override void ExecuteAction(GameContext gameContext)
        {
            throw new NotImplementedException();
        }
    }
}