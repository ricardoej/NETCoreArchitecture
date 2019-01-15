using data.core;

namespace repository.core
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepositoryByEntity<T>() where T : BaseEntity;
        T GetRepository<T>() where T : IRepository;
        void Save();
    }
}