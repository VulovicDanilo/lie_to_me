using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPI2.Models;

namespace WebAPI2.Repositories.TownRoleRepo
{
    public class TownRoleRepository : Repository<TownRole>, ITownRoleRepository
    {
        public TownRoleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public string AbilitiesAndGoal(int id)
        {
            TownRole townRole = DbSet.Find(id);
            return "Abilities: " + townRole.Abilities + "\n\nGoal: " + townRole.Goal;
        }
    }
}