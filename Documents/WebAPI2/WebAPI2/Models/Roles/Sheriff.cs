using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI2.Models.Roles
{
    public class Sheriff : Role
    {
        public Sheriff() : base(RoleName.Sheriff, Alignment.Town, 4)
        {

        }
        public override void ExecuteAction(GameContext gameContext)
        {
            throw new NotImplementedException();
        }
    }
}