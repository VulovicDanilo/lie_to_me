using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAPI2.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string UserName { get; set; }
        public string ImagePath { get; set; }

    }
}