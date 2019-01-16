using Autofac;
using repository.core;
using System.Reflection;
using System.Linq;

namespace repository
{
    public class RepositoryModule: Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterRepositories(builder);
        }

        private void RegisterRepositories(ContainerBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly)
                .PublicOnly()
                .Where(t => t.GetInterfaces().Contains(typeof(IRepository)))
                .AsImplementedInterfaces().Where(t => !t.IsGenericType);
        }
    }
}
