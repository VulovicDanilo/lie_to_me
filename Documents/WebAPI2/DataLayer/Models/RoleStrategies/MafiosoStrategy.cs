using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Models.Roles
{
    public class MafiosoStrategy : RoleStrategy
    {
        public MafiosoStrategy()
            : base(RoleName.Mafioso, Alignment.Mafia, 5)
        {

        }

        public override void ExecuteAction(Game game)
        {
            throw new NotImplementedException();
        }

        public new static MafiosoStrategy CreateRoleStrategy() { return new MafiosoStrategy(); }
    }
}