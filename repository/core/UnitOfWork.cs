using System;
using System.Collections.Generic;
using data.core;

namespace repository.core
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext context;
        private Dictionary<Type, IRepository> repositories;

        public UnitOfWork(ApplicationContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, IRepository>();
        }

        public IRepository<T> GetRepository<T>() where T : BaseEntity
        {
            var entityType = typeof(T);
            if (!repositories.ContainsKey(entityType))
                repositories.Add(entityType, new Repository<T>(context));

            return (IRepository<T>)repositories[entityType];
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}