using System.ComponentModel.DataAnnotations;

namespace WebAPI2.Models
{
    public abstract class TownRole
    {
        protected TownRole(string name, Alignment alignment, string abilities, string goal)
        {
            Name = name;
            Alignment = alignment;
            Abilities = abilities;
            Goal = goal;
        }

        public int TownRoleID { get; set; }
        public string Name { get; set; }
        public Alignment Alignment {get; set;}
        public string Abilities { get; set; }
        public string Goal { get; set; }


        public virtual void Vote()
        {

        }
        public virtual void Trial()
        {

        }

    }
}