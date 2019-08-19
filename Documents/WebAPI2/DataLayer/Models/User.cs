using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataLayer.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [StringLength(256)]
        [Index(IsUnique = true)]
        [Required(AllowEmptyStrings = true)]
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string ImagePath { get; set; }

        public User()
        {
        }

    }
}