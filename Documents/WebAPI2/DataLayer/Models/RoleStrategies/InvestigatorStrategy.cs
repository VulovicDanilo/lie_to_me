using DataLayer.DTOs;
using DataLayer.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class InvestigatorStrategy :RoleStrategy
    {
        public InvestigatorStrategy()
            :base(RoleName.Investigator, Alignment.Town, 4, "investigate one person each night for a clue to their role", "defeat all mafia")
        {
            this.Offence = Offence.None;
            this.Defence = Defence.None;
        }

        public override void ExecuteAction(Game game, ExecuteActionModel model)
        {
            var me = game.Players.Find(x => x.Number == model.Who);
            var toPlayer = game.Players.Find(x => x.Number == model.To);

            var roles = Game.GetRandomRoles(toPlayer.Role.RoleName).ToList();
            roles.Add(toPlayer.Role.RoleName);
            roles.Shuffle();

            string log = toPlayer.FakeName + " is: ";
            foreach(var rolename in roles)
            {
                log += rolename.ToString().ToLower() + " / ";
            }
            log = log.Substring(0, log.Length - 3);
            me.AddLog(log);

        }
    }
}