using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models
{
    public abstract class Role
    {
        public RoleName RoleName { get; private set; }
        public Alignment Alignment { get; private set; }
        public abstract void ExecuteAction(GameContext gameContext);
        public Player SelectedPlayer { get; set; }
        public int Priority { get; private set; }

        protected Role(RoleName roleName, Alignment alignment, int priority)
        {
            RoleName = roleName;
            Alignment = alignment;
            Priority = priority;
        }
    }
}