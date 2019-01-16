using data;
using service.core;

namespace service.services
{
    public interface IUsuarioService: ICRUDService<Usuario>
    {
        Usuario GetByLogin(string login);
        Usuario Autenticar(string login, string senha);
    }
}
