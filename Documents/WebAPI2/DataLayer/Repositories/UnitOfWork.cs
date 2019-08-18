using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataLayer.Repositories
{
    public class UnitOfWork : IDisposable
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private Repository<Game> gameRepository;
        private Repository<User> userRepository;
        private Repository<Player> playerRepository;

        public Repository<Game> GameRepository
        {
            get
            {
                if (gameRepository == null)
                {
                    gameRepository = new Repository<Game>(context);
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