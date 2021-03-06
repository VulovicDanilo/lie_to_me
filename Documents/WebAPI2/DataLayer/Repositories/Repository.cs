﻿using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace DataLayer.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected ApplicationDbContext Context { get; set; }
        protected DbSet<T> DbSet { get; set; }
        public Repository(ApplicationDbContext context)
        {
            Context = context ?? throw new ArgumentNullException("null dbContext in Repository contructor");
            DbSet = context.Set<T>();
        }

        public virtual List<T> List
        {
            get
            {
                return DbSet.ToList();
            }
        }

        public void Add(T entity)
        {
            DbEntityEntry dbEntityEntry = Context.Entry(entity);
            if (dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                DbSet.Add(entity);
            }
        }
        public T Find(int id)
        {
            return DbSet.Find(id);
        }

        public void Update(T entity)
        {
            DbEntityEntry dbEntityEntry = Context.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;
        }
        public void Delete(T entity)
        {
            DbEntityEntry dbEntityEntry = Context.Entry(entity);
            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                DbSet.Attach(entity);
                DbSet.Remove(entity);
            }
        }

        public void Delete(int id)
        {
            T entity = Find(id);
            if (entity == null)
                return;
            Delete(entity);
        }
    }
}