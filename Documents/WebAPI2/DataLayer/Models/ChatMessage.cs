using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models
{
    public class ChatMessage
    {
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public string Name { get; set; }
        public GameState GameState { get; set; }

        public ChatMessage(string content, string name, GameState gameState)
        {
            Content = content;
            Name = name;
            Time = DateTime.Now;
            GameState = GameState;
        }
    }
}