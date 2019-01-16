using Autofac;
using service.core;
using System.Reflection;
using System.Linq;
using service.services;
using service.services.impl;

namespace service
{
    public class ServiceModule: Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //RegisterServices(builder);
            builder.RegisterType<UsuarioService>().As<IUsuarioService>();
        }

        private void RegisterServices(ContainerBuilder builder)
        {
            var assembly = Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly)
                .PublicOnly()
                .Where(t => t.IsInterface && !t.IsGenericType && t.GetInterfaces().Contains(typeof(IService)));
        }
    }
}
