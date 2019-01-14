using data.core;

namespace repository.core
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T: BaseEntity;
        void Save();
    }
}