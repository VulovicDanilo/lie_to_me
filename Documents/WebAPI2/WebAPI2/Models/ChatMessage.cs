using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI2.Models
{
    public class ChatMessage
    {
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public Player Id { get; set; }
    }
}