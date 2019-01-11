using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPI2.Models;

namespace WebAPI2.Roles
{
    public class VeteranRole : TownRole
    {
        public VeteranRole()
            : base("Veteran", 
                  Alignment.Town, 
                  "Can go on alert up to 3 times during the night.\nWhile on alert,you will kill anyone that visits you.",
                  "Lynch every criminal and evildoer.")
        {

        }
    }
}