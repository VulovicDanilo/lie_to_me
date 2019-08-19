using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public User Find(string username)
        {
            return DbSet.Where(x => x.UserName == username).FirstOrDefault();
        }

    }
}
