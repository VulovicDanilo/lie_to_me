using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI2.Models.Roles
{
    public class Lookout : Role
    {
        public Lookout()
            : base(RoleName.Lookout, Alignment.Town, 6)
        {

        }

        public override void ExecuteAction(GameContext gameContext)
        {
            throw new NotImplementedException();
        }
    }
}