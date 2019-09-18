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
        public string Description { get; set; }
        public string Goal { get; set; }

        protected RoleStrategy(RoleName roleName, Alignment alignment, int priority, string description, string goal)
        {
            RoleName = roleName;
            Alignment = alignment;
            Priority = priority;
            Description = description;
            Goal = goal;
        }
        public RoleStrategy()
        {

        }
    }
}