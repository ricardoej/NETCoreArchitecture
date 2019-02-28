using entities.core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace repository.core
{
    public abstract class Repository<T> where T : BaseEntity
    {
        protected readonly ApplicationContext context;
        protected readonly DbSet<T> query;

        public Repository(ApplicationContext context)
        {
            this.context = context;
            query = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return query.AsEnumerable();
        }
 
        public T Get(long id)
        {
            return query.SingleOrDefault(s => s.Id == id);
        }

        public void Create(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            query.Add(entity);
        }
 
        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            query.Update(entity);
        }
 
        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            query.Remove(entity);
        }
    }
}