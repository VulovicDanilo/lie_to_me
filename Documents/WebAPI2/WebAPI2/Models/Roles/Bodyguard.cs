using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI2.Models.Roles
{
    public class Bodyguard : Role
    {
        public Bodyguard() : base(RoleName.BodyGuard, Alignment.Town, 3)
        {
            
        }

        public override void ExecuteAction(GameContext gameContext)
        {
            throw new NotImplementedException();
        }


    }
}