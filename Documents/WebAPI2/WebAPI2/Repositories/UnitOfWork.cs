using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPI2.Models;
using WebAPI2.Repositories.GameRepo;
using WebAPI2.Repositories.TownRoleRepo;

namespace WebAPI2.Repositories
{
    public class UnitOfWork : IDisposable
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private Repository<Game> gameRepository;
        private Repository<User> userRepository;
        private Repository<Player> playerRepository;
        private Repository<TownRole> townRoleRepository;

        public Repository<Game> GameRepository
        {
            get
            {
                if (gameRepository == null)
                {
                    gameRepository = new GameRepository(context);
                }
                return gameRepository;
            }
        }
        public Repository<User> UserRepository
        {
            get
            {
                if (userRepository == null)
                {
                    userRepository = new Repository<User>(context);
                }
                return userRepository;
            }
        }
        public Repository<Player> PlayerRepository
        {
            get
            {
                if (playerRepository == null)
                {
                    playerRepository = new Repository<Player>(context);
                }
                return playerRepository;
            }
        }
        public Repository<TownRole> TownRoleRepository
        {
            get
            {
                if (townRoleRepository == null)
                {
                    townRoleRepository = new TownRoleRepository(context);
                }
                return townRoleRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        // Dispose pattern

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}