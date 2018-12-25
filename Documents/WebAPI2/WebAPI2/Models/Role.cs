using System.ComponentModel.DataAnnotations;

namespace WebAPI2.Models
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }
    }
}