using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class Veteran : Role
    {
        public Veteran() : base(RoleName.Veteran, Alignment.Town, 1)
        {

        }
        public override void ExecuteAction(GameContext gameContext)
        {
            throw new NotImplementedException();
        }
    }
}