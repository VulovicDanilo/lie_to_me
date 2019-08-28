using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DataLayer.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }
        [Column(name: "StartTime")]
        public DateTime? StartTime { get; set; }
        [Column(name: "EndTime")]
        public DateTime? EndTime { get; set; }
        public Alignment? Winner { get; set; } 
        public List<Player> Players { get; set; }
        [ForeignKey("Owner")]
        public int? Owner_Id { get; set; }
        public Player Owner { get; set; }
        public GameMode GameMode { get; set; }


        [NotMapped]
        public readonly int MAX_PLAYERS = 10;
        [NotMapped]
        public GameState GameState { get; set; }
        [NotMapped]
        public List<Player> Winners { get; set; }
        [NotMapped]
        public IModel Channel { get; set; }


        public Game()
        {
            Players = new List<Player>();
            Owner = null;
        }
        public Game(Player owner)
            :base()
        {
            Owner = owner;
        }

    }
}