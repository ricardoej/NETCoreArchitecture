using domain.core;
using domain.exceptions;
using domain.repositories;
using domain.request;
using domain.response;
using entities;
using System;
using System.Text;

namespace domain.services
{
    public class InserirUsuarioService : IExecutar<InserirUsuarioRequest, InserirUsuarioResponse>
    {
        private readonly IUsuarioRepository usuarioRepository;

        public InserirUsuarioService(IUsuarioRepository usuarioRepository)
        {
            this.usuarioRepository = usuarioRepository;
        }

        public InserirUsuarioResponse Executar(InserirUsuarioRequest parametro)
        {
            if (string.IsNullOrWhiteSpace(parametro.Login))
                return InsercaoInvalida(new ArgumentNullException("Login"));

            if (string.IsNullOrWhiteSpace(parametro.Email))
                return InsercaoInvalida(new ArgumentNullException("Email"));

            if (string.IsNullOrWhiteSpace(parametro.Nome))
                return InsercaoInvalida(new ArgumentNullException("Nome"));

            if (string.IsNullOrWhiteSpace(parametro.Senha))
                return InsercaoInvalida(new ArgumentNullException("Senha"));

            if (usuarioRepository.BuscarPorLogin(parametro.Login) != null)
                return InsercaoInvalida(new UsuarioJaExisteException(parametro.Login));

            CreatePasswordHash(parametro.Senha, out byte[] senhaHash, out byte[] senhaSalt);

            var usuario = new Usuario()
            {
                Nome = parametro.Nome,
                Login = parametro.Login,
                Email = parametro.Email,
                SenhaHash = senhaHash,
                SenhaSalt = senhaSalt
            };

            usuario = usuarioRepository.InserirUsuario(usuario);

            return new InserirUsuarioResponse(usuario);
        }

        private InserirUsuarioResponse InsercaoInvalida(Exception erro)
        {
            return new InserirUsuarioResponse(erro);
        }

        private void CreatePasswordHash(string senha, out byte[] senhaHash, out byte[] senhaSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                senhaSalt = hmac.Key;
                senhaHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(senha));
            }
        }
    }
}
