using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class SerialKiller : Role
    {
        public SerialKiller() : base(RoleName.SerialKiller, Alignment.Neutral, 5)
        {

        }
        public override void ExecuteAction(GameContext gameContext)
        {
            throw new NotImplementedException();
        }
    }
}