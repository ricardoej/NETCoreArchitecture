using data;
using data.request;
using service.core;

namespace service.services
{
    public interface IUsuarioService: ICRUDService<Usuario>
    {
        void Insert(UsuarioRequest usuarioRequest);
        Usuario GetByLogin(string login);
        Usuario Autenticar(string login, string senha);
    }
}
