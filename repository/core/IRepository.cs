using System.Collections.Generic;
using data.core;
using Microsoft.EntityFrameworkCore;

namespace repository.core
{
    public interface IRepository
    {
    }

    public interface IRepository<T>: IRepository where T : BaseEntity
    {
        DbSet<T> Query { get; }
        IEnumerable<T> GetAll();
        T Get(long id);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}