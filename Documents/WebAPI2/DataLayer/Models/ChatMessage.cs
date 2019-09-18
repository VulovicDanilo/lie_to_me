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
        public int PlayerId { get; set; }
        public int GameId { get; set; }
        public GameState GameState { get; set; }

        public ChatMessage(string content, int playerId, int gameId, GameState gameState)
        {
            Content = content;
            PlayerId = playerId;
            Time = DateTime.Now;
            GameId = gameId;
            GameState = GameState;
        }
    }
}