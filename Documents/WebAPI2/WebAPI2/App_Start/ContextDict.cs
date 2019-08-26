using DataLayer.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI2.App_Start
{
    public static class ContextDict
    {
        private static ConcurrentDictionary<int, GameContext> Contexts { get; set; }
            = new ConcurrentDictionary<int, GameContext>();

        public static GameContext GetOrAdd(int id)
        {
           return Contexts.GetOrAdd(id, CreateContext);
        }

        private static GameContext CreateContext(int arg)
        {
            return new GameContext();
        }

        public static void Remove(int id)
        {
            Contexts.TryRemove(id, out _);
        }
    }
}