using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DTOs
{
    public class DeadPlayer
    {
        public string FakeName { get; set; }
        public RoleName RoleName { get; set; }
        public int Number { get; set; }
        public Alignment Alignment { get; set; }
        public string LastWill { get; set; }
        public int PlayerId { get; private set; }
        public string ImagePath { get; private set; }

        public static DeadPlayer ToDTO(Player player)
        {
            var inGamePlayer = new DeadPlayer()
            {
                PlayerId = player.Id,
                FakeName = player.FakeName,
                ImagePath = player.User.ImagePath,
                Number = player.Number,
            };
            return inGamePlayer;
        }
        public static List<DeadPlayer> ToDTOs(List<Player> players)
        {
            var list = new List<DeadPlayer>();
            foreach (var player in players)
            {
                list.Add(ToDTO(player));
            }
            return list;
        }
    }
}
