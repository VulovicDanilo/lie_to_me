using System.ComponentModel.DataAnnotations;

namespace WebAPI2.Models
{
    public class TownRole
    {
        public int TownRoleID { get; set; }
        public string Name { get; set; }
        public Alignment Alignment {get; set;}
        public string Description { get; set; }
        public string Abilities { get; set; }
        public string Goal { get; set; }

        public void Vote()
        {
        }
        public void Trial()
        {

        }

    }
}