using System;
using System.Collections.Generic;
using data.core;

namespace repository.core
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext context;
        private readonly IServiceProvider provider;
        private Dictionary<Type, IRepository> repositories;

        public UnitOfWork(ApplicationContext context, IServiceProvider provider)
        {
            this.context = context;
            this.provider = provider;
            this.repositories = new Dictionary<Type, IRepository>();
        }

        public IRepository<T> GetRepositoryByEntity<T>() where T : BaseEntity
        {
            var entityType = typeof(T);
            if (!repositories.ContainsKey(entityType))
                repositories.Add(entityType, new Repository<T>(context));

            return (IRepository<T>)repositories[entityType];
        }

        public T GetRepository<T>() where T : IRepository
        {
            var entityType = typeof(T);
            if (!repositories.ContainsKey(entityType))
                repositories.Add(entityType, (IRepository)provider.GetService(entityType));

            return (T)repositories[entityType];
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}