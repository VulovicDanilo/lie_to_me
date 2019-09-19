using DataLayer.DTOs;
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
        public virtual void ExecuteAction(Game game, ExecuteActionModel model){ }
        public Player SelectedPlayer { get; set; }
        public int Priority { get; set; }
        public string Description { get; set; }
        public string Goal { get; set; }
        public Offence Offence { get; set; }
        public Defence Defence { get; set; }
        public bool CanVisit { get; set; } = true;

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