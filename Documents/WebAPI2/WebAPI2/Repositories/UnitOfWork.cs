using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPI2.Models;
using WebAPI2.Repositories.GameRepo;

namespace WebAPI2.Repositories
{
    public class UnitOfWork : IDisposable
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private Repository<Game> gameRepository;

        public Repository<Game> GameRepository
        {
            get
            {
                if (gameRepository==null)
                {
                    gameRepository = new GameRepository(context);
                }
                return gameRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

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