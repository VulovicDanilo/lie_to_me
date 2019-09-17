using DataLayer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DTOs
{
    public class InGamePlayer
    {
        public int Number { get; set; }
        public string FakeName { get; set; }
        public string ImagePath { get; set; }
        public bool Alive { get; set; }
        public int PlayerId { get; set; }

        public static InGamePlayer ToDTO(Player player)
        {
            var inGamePlayer = new InGamePlayer()
            {
                PlayerId = player.Id,
                FakeName = player.FakeName,
                ImagePath = player.User.ImagePath,
                Number = player.Number,
                Alive = player.Alive,
            };
            return inGamePlayer;
        }
        public static List<InGamePlayer> ToDTOs(List<Player> players)
        {
            var list = new List<InGamePlayer>();
            foreach(var player in players)
            {
                list.Add(ToDTO(player));
            }
            return list;
        }

        public InGamePlayer()
        {

        }
    }
}
