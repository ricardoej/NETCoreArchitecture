using System.Collections.Generic;
using data.core;

namespace repository.core
{
    public interface IRepository
    {
    }

    public interface IRepository<T>: IRepository where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        T Get(long id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Remove(T entity);
    }
}