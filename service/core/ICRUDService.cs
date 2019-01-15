using data.core;
using System;
using System.Collections.Generic;
using System.Text;

namespace service.core
{
    public interface ICRUDService<T>: IService where T: BaseEntity
    {
        IEnumerable<T> GetAll();
        T Get(long id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Remove(T entity);
    }
}
