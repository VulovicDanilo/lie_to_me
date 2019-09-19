using DataLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class DoctorStrategy : RoleStrategy
    {
        public DoctorStrategy()
            : base(RoleName.Doctor, Alignment.Town, 3, "heal one person each night, preventing them from dying", "defeat all mafia")
        {
            this.Offence = Offence.None;
            this.Defence = Defence.None;
        }

        public override void ExecuteAction(Game game, ExecuteActionModel model)
        {

        }
    }
}