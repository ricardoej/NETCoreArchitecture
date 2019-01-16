using data;
using data.request;
using repository.core;
using service.core;
using System;
using System.Linq;

namespace service.services.impl
{
    public class UsuarioService : CRUDService<Usuario>, IUsuarioService
    {
        public UsuarioService(IUnitOfWork unitOfWork): base(unitOfWork)
        {}

        public Usuario Autenticar(string login, string senha)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(senha))
                return null;

            var usuario = GetByLogin(login);
            
            if (usuario == null)
                return null;
            
            if (!VerifyPasswordHash(senha, usuario.SenhaHash, usuario.SenhaSalt))
                return null;
            
            return usuario;
        }

        private static bool VerifyPasswordHash(string senha, byte[] storedHash, byte[] storedSalt)
        {
            if (senha == null) throw new ArgumentNullException("senha");
            if (string.IsNullOrWhiteSpace(senha)) throw new ArgumentException("Valor não pode ser vazio.", "senha");
            if (storedHash.Length != 64) throw new ArgumentException("Tamanho inválido (64 bytes esperado).", "senhaHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Tamanho inválido (128 bytes esperado).", "senhaSalt");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senha));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        public Usuario GetByLogin(string login)
        {
            return unitOfWork.GetRepositoryByEntity<Usuario>().Query.Where(u => u.Login == login).FirstOrDefault();
        }

        public void Insert(UsuarioRequest usuarioRequest)
        {
            if (string.IsNullOrWhiteSpace(usuarioRequest.Senha))
                throw new Exception("Senha é requerida");

            if (GetByLogin(usuarioRequest.Login) != null)
                throw new Exception("Login \"" + usuarioRequest.Login + "\" já existe");

            CreatePasswordHash(usuarioRequest.Senha, out byte[] senhaHash, out byte[] senhaSalt);

            var usuario = new Usuario()
            {
                Nome = usuarioRequest.Nome,
                Login = usuarioRequest.Login,
                Email = usuarioRequest.Email,
                SenhaHash = senhaHash,
                SenhaSalt = senhaSalt
            };

            base.Insert(usuario);
        }

        private void CreatePasswordHash(string senha, out byte[] senhaHash, out byte[] senhaSalt)
        {
            if (senha == null) throw new ArgumentNullException("senha");
            if (string.IsNullOrWhiteSpace(senha)) throw new ArgumentException("Valor não pode ser vazio.", "senha");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                senhaSalt = hmac.Key;
                senhaHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senha));
            }
        }

        public override void Insert(Usuario entity)
        {
            throw new InvalidOperationException("Não é possível utilizar esse método. Utilizar Insert(UsuarioRequest entity, string senha)");
        }
    }
}
