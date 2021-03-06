﻿using DataLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class JesterStrategy : RoleStrategy
    {
        public JesterStrategy()
            : base(RoleName.Jester, Alignment.Neutral, 10, "trick the town into voting against you", "be hanged")
        {
            this.Offence = Offence.None;
            this.Defence = Defence.None;
            this.CanVisit = false;
        }

        public override void ExecuteAction(Game game, ExecuteActionModel model)
        {
            
        }
    }
}