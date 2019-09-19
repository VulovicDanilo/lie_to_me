using DataLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class SheriffStrategy : RoleStrategy
    {
        public SheriffStrategy() : base(RoleName.Sheriff, Alignment.Town, 4, "check one person each night for suspicious activity", "defeat all mafia")
        {
            this.Offence = Offence.None;
            this.Defence = Defence.None;
        }
        public override void ExecuteAction(Game game, ExecuteActionModel model)
        {
            var me = game.Players.Find(x => x.Number == model.Who);
            var toPlayer = game.Players.Find(x => x.Number == model.To);
            string suspicious = toPlayer.Suspicious ? "suspicious" : "not suspicious";
            string log = toPlayer.FakeName + " is " + suspicious;
            me.AddLog(log);
        }
    }
}