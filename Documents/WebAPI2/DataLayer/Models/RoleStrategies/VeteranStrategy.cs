﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class VeteranStrategy : RoleStrategy
    {
        public VeteranStrategy() : base(RoleName.Veteran, Alignment.Town, 1)
        {

        }
        public override void ExecuteAction(Game game)
        {
            throw new NotImplementedException();
        }

        public new static VeteranStrategy CreateRoleStrategy() { return new VeteranStrategy(); }
    }
}