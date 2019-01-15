using data.core;
using repository.core;
using System;
using System.Collections.Generic;
using System.Text;

namespace service.core
{
    public abstract class CRUDService<T> : BaseService, ICRUDService<T> where T : BaseEntity
    {
        public CRUDService(IUnitOfWork unitOfWork): base(unitOfWork)
        {}

        public virtual void Delete(T entity)
        {
            unitOfWork.GetRepositoryByEntity<T>().Delete(entity);
            unitOfWork.Save();
        }

        public virtual T Get(long id)
        {
            return unitOfWork.GetRepositoryByEntity<T>().Get(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return unitOfWork.GetRepositoryByEntity<T>().GetAll();
        }

        public virtual void Insert(T entity)
        {
            unitOfWork.GetRepositoryByEntity<T>().Create(entity);
            unitOfWork.Save();
        }

        public virtual void Remove(T entity)
        {
            unitOfWork.GetRepositoryByEntity<T>().Delete(entity);
            unitOfWork.Save();
        }

        public virtual void Update(T entity)
        {
            unitOfWork.GetRepositoryByEntity<T>().Update(entity);
            unitOfWork.Save();
        }
    }
}
