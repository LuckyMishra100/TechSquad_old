﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TechSquad.Application.Data;
using TechSquad.Models.Models;
using TechSquad.Services.Data.IRepository;

namespace TechSquad.Services.Data.Repository
{
    public class GenericRepository<T>: IGenericRepository<T> where T : class
    {
        internal DbSet<T> dbSet;
        private readonly ApplicationDbContext _context = null;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            this.dbSet = _context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
            return query.ToList();
        }

        public virtual T GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public async Task<T> AddAsync(T entity)
        {
            dbSet.Add(entity);
            return entity;
        }

        public virtual void DeleteByID(object id)
        {
            T entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(T entityToDelete)
        {

            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(T entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);

            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public async Task<T> UpdateAsync(T entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);

            _context.Entry(entityToUpdate).State = EntityState.Modified;
            return entityToUpdate;
        }

        public async Task<T> DeleteAsync(T entityToDelete)
        {

            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
            return entityToDelete;
        }

    }
}
