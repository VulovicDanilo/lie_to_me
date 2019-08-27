using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DTOs
{
    public class InGamePlayer
    {
        public int Number { get; private set; }
        public string FakeName { get; private set; }
        public string ImagePath { get; private set; }
        public bool Alive { get; private set; }
        public int PlayerId { get; private set; }

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
