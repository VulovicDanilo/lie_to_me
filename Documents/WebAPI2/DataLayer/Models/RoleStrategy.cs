using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models
{
    public abstract class RoleStrategy
    {
        public RoleName RoleName { get; private set; }
        public Alignment Alignment { get; private set; }
        public abstract void ExecuteAction(Game game);
        public Player SelectedPlayer { get; set; }
        public int Priority { get; private set; }

        protected RoleStrategy(RoleName roleName, Alignment alignment, int priority)
        {
            RoleName = roleName;
            Alignment = alignment;
            Priority = priority;
        }

        public new static RoleStrategy CreateRoleStrategy() { return null; }
    }
}