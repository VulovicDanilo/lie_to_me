using System.ComponentModel.DataAnnotations;

namespace WebAPI2.Models
{
    public class TownRole:IEntity
    {
        public int TownRoleID { get; set; }

        public int ID
        {
            get
            {
                return TownRoleID;
}
        }
    }
}