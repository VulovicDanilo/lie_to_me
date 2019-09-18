using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models
{
    public class RoleStrategy
    {
        public RoleName RoleName { get; set; }
        public Alignment Alignment { get; set; }
        public virtual void ExecuteAction(Game game) { }
        public Player SelectedPlayer { get; set; }
        public int Priority { get; set; }

        protected RoleStrategy(RoleName roleName, Alignment alignment, int priority)
        {
            RoleName = roleName;
            Alignment = alignment;
            Priority = priority;
        }
        public RoleStrategy()
        {

        }
    }
}