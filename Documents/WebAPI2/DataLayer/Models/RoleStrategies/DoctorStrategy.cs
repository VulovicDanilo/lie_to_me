using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class DoctorStrategy : RoleStrategy
    {
        public DoctorStrategy()
            : base(RoleName.Doctor, Alignment.Town, 3)
        {

        }

        public override void ExecuteAction(GameContext gameContext)
        {
            throw new NotImplementedException();
        }
    }
}