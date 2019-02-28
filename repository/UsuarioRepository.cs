using domain.repositories;
using entities;
using repository.core;
using System.Linq;

namespace repository
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ApplicationContext context): base(context) { }

        public Usuario BuscarPorLogin(string login)
        {
            return GetAll().Where(usuario => usuario.Login == login).SingleOrDefault();
        }

        public Usuario InserirUsuario(Usuario usuario)
        {
            Create(usuario);
            context.SaveChanges();
            return usuario;
        }
    }
}
