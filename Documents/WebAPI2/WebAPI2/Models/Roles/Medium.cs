using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI2.Models.Roles
{
    public class Medium : Role
    {
        public Medium() : base(RoleName.Medium, Alignment.Town, 10)
        {

        }
        public override void ExecuteAction(GameContext gameContext)
        {
            
        }
    }
}