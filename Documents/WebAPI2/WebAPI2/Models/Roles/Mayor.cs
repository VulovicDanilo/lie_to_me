using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI2.Models.Roles
{
    public class Mayor : Role
    {
        public Mayor()
            : base(RoleName.Mayor, Alignment.Town, 10)
        {

        }

        public override void ExecuteAction(GameContext gameContext)
        {
            throw new NotImplementedException();
        }
    }
}