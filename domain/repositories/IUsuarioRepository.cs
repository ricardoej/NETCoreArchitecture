using entities;

namespace domain.repositories
{
    public interface IUsuarioRepository
    {
        Usuario BuscarPorLogin(string login);

        Usuario InserirUsuario(Usuario usuario);
    }
}
