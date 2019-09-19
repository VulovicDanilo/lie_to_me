using DataLayer.DTOs;

namespace DataLayer.Models.Roles
{
    public class LookoutStrategy : RoleStrategy
    {
        public LookoutStrategy()
            : base(RoleName.Lookout, Alignment.Town, 6, "watch one person at night to see who visits them", "defeat all mafia")
        {
            this.Offence = Offence.None;
            this.Defence = Defence.None;
        }

        public override void ExecuteAction(Game game, ExecuteActionModel model)
        {
            var me = game.Players.Find(x => x.Number == model.Who);
            var toPlayer = game.Players.Find(x => x.Number == model.To);
            string log = toPlayer.FakeName + " was wisited by: ";
            foreach (var visitor in toPlayer.Visitors)
            {
                log += visitor.FakeName + ", ";
            }
            log = log.Substring(0, log.Length - 2);
            me.AddLog(log);
        }
    }
}
