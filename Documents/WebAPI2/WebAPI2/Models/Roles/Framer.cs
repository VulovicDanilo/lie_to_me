﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI2.Models.Roles
{
    public class Framer : Role
    {
        public Framer()
            : base(RoleName.Framer, Alignment.Mafia, 2)
        {
            
        }

        public override void ExecuteAction(GameContext gameContext)
        {
            throw new NotImplementedException();
        }
    }
}