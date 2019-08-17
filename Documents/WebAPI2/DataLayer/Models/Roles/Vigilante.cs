using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class Vigilante : Role
    {
        public Vigilante() : base(RoleName.Vigilante, Alignment.Town, 5)
        {
        }

        public override void ExecuteAction(GameContext gameContext)
        {
            throw new NotImplementedException();
        }
    }
}