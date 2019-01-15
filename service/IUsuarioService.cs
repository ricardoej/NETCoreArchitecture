using data.portalseguranca;
using service.core;

namespace service
{
    public interface IUsuarioService: ICRUDService<Usuario>
    {
        Usuario GetByLogin(string login);
        Usuario Autenticar(string login, string senha);
    }
}
