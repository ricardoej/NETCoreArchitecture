using domain.core;
using domain.exceptions;
using domain.repositories;
using domain.request;
using domain.response;
using System;

namespace domain.services
{
    public class AutenticarUsuarioService : IExecutar<AutenticarRequest, AutenticarResponse>
    {
        private readonly IUsuarioRepository usuarioRepository;

        public AutenticarUsuarioService(IUsuarioRepository usuarioRepository)
        {
            this.usuarioRepository = usuarioRepository;
        }

        public AutenticarResponse Executar(AutenticarRequest parametro)
        {
            if (string.IsNullOrWhiteSpace(parametro.Login))
                return LoginInvalido(new ArgumentNullException("Login"));

            if (string.IsNullOrWhiteSpace(parametro.Senha))
                return LoginInvalido(new ArgumentNullException("Senha"));

            var usuario = usuarioRepository.BuscarPorLogin(parametro.Login);

            if (usuario == null)
                return LoginInvalido(new UsuarioNaoEncontradoException());

            if (!VerificaPasswordHash(parametro.Senha, usuario.SenhaHash, usuario.SenhaSalt))
                return LoginInvalido(new SenhaIncorretaException());

            return LoginValido(usuario);
        }

        private AutenticarResponse LoginInvalido(Exception erro)
        {
            return new AutenticarResponse(erro);
        }

        private AutenticarResponse LoginValido(entities.Usuario usuario)
        {
            return new AutenticarResponse(usuario);
        }

        private bool VerificaPasswordHash(string senha, byte[] storedHash, byte[] storedSalt)
        {
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
    }
}
