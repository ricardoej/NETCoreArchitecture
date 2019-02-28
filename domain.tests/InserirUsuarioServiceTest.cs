using domain.exceptions;
using domain.repositories;
using domain.request;
using domain.services;
using entities;
using NSubstitute;
using System;
using Xunit;

namespace domain.tests
{
    public class InserirUsuarioServiceTest
    {
        private readonly InserirUsuarioService service;
        private readonly Usuario usuario;

        private readonly string LOGIN = "ricardo";
        private readonly string EMAIL = "ricardo@mail.com";
        private readonly string NOME = "Ricardo";
        private readonly byte[] SENHA_HASH = new byte[64] { 88, 141, 53, 147, 195, 221, 120, 76, 133, 58, 75, 245, 154, 66, 171, 36, 195, 218, 184, 30, 251, 4, 146, 2, 74, 28, 190, 46, 240, 197, 64, 240, 33, 106, 77, 229, 80, 236, 72, 165, 33, 137, 44, 164, 193, 126, 233, 148, 157, 146, 39, 168, 219, 240, 199, 150, 153, 185, 26, 243, 106, 165, 54, 54 };
        private readonly byte[] SENHA_SALT = new byte[128] { 9, 69, 222, 255, 109, 36, 87, 75, 120, 46, 116, 133, 76, 189, 194, 33, 227, 10, 48, 7, 60, 38, 59, 250, 154, 170, 230, 99, 122, 172, 45, 61, 167, 171, 158, 209, 239, 73, 208, 95, 65, 119, 127, 70, 30, 104, 235, 48, 233, 248, 103, 20, 71, 196, 32, 160, 255, 205, 82, 36, 145, 105, 43, 66, 155, 38, 84, 44, 73, 147, 39, 235, 5, 235, 133, 114, 69, 228, 172, 147, 1, 8, 14, 55, 133, 43, 140, 187, 60, 103, 72, 28, 145, 69, 66, 122, 93, 10, 53, 204, 151, 219, 103, 156, 87, 60, 234, 160, 171, 83, 227, 171, 42, 157, 18, 30, 69, 61, 109, 93, 36, 135, 108, 110, 242, 119, 217, 83 };


        public InserirUsuarioServiceTest()
        {
            var repositoryMock = Substitute.For<IUsuarioRepository>();
            usuario = new Usuario()
            {
                Id = 1,
                Login = LOGIN,
                Email = EMAIL,
                Nome = NOME,
                SenhaHash = SENHA_HASH,
                SenhaSalt = SENHA_SALT
            };
            repositoryMock.BuscarPorLogin(LOGIN).Returns(c => null);
            repositoryMock.InserirUsuario(usuario).ReturnsForAnyArgs(usuario);
            service = new InserirUsuarioService(repositoryMock);
        }

        [Fact]
        public void RetornaSucessoUsuarioInserido()
        {
            var parametro = new InserirUsuarioRequest()
            {
                Login = usuario.Login,
                Senha = "123",
                Email = usuario.Email,
                Nome = usuario.Nome
            };

            var resposta = service.Executar(parametro);

            Assert.True(resposta.IsInserido);
            Assert.Equal(LOGIN, resposta.Usuario.Login);
            Assert.Equal(EMAIL, resposta.Usuario.Email);
            Assert.Equal(NOME, resposta.Usuario.Nome);
            Assert.Equal(SENHA_HASH, resposta.Usuario.SenhaHash);
            Assert.Equal(SENHA_SALT, resposta.Usuario.SenhaSalt);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void RetornaExceçãoLoginNullOrEmBranco(string login)
        {
            var parametro = new InserirUsuarioRequest()
            {
                Login = login,
                Senha = "123",
                Email = usuario.Email,
                Nome = usuario.Nome
            };

            var resposta = service.Executar(parametro);

            Assert.False(resposta.IsInserido);
            Assert.IsType<ArgumentNullException>(resposta.Erro);
            Assert.Equal("Login", ((ArgumentNullException)resposta.Erro).ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void RetornaExceçãoEmailNullOrEmBranco(string email)
        {
            var parametro = new InserirUsuarioRequest()
            {
                Login = "ricardo",
                Senha = "123",
                Email = email,
                Nome = usuario.Nome
            };

            var resposta = service.Executar(parametro);

            Assert.False(resposta.IsInserido);
            Assert.IsType<ArgumentNullException>(resposta.Erro);
            Assert.Equal("Email", ((ArgumentNullException)resposta.Erro).ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void RetornaExceçãoNomeNullOrEmBranco(string nome)
        {
            var parametro = new InserirUsuarioRequest()
            {
                Login = "ricardo",
                Senha = "123",
                Email = "ricardo@mail.com",
                Nome = nome
            };

            var resposta = service.Executar(parametro);

            Assert.False(resposta.IsInserido);
            Assert.IsType<ArgumentNullException>(resposta.Erro);
            Assert.Equal("Nome", ((ArgumentNullException)resposta.Erro).ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void RetornaExceçãoSenhaNullOrEmBranco(string senha)
        {
            var parametro = new InserirUsuarioRequest()
            {
                Login = usuario.Login,
                Senha = senha,
                Email = usuario.Email,
                Nome = usuario.Nome
            };

            var resposta = service.Executar(parametro);

            Assert.False(resposta.IsInserido);
            Assert.IsType<ArgumentNullException>(resposta.Erro);
            Assert.Equal("Senha", ((ArgumentNullException)resposta.Erro).ParamName);
        }

        [Fact]
        public void RetornaExceçãoUsuarioJaExiste()
        {
            var repositoryMock = Substitute.For<IUsuarioRepository>();
            var usuario = new Usuario()
            {
                Id = 1,
                Login = LOGIN,
                Email = EMAIL,
                Nome = NOME,
                SenhaHash = SENHA_HASH,
                SenhaSalt = SENHA_SALT
            };
            repositoryMock.InserirUsuario(usuario).Returns(usuario);
            repositoryMock.BuscarPorLogin(LOGIN).Returns(usuario);
            var service = new InserirUsuarioService(repositoryMock);

            var parametro = new InserirUsuarioRequest()
            {
                Login = usuario.Login,
                Senha = "123",
                Email = usuario.Email,
                Nome = usuario.Nome
            };

            var resposta = service.Executar(parametro);

            Assert.False(resposta.IsInserido);
            Assert.IsType<UsuarioJaExisteException>(resposta.Erro);
            Assert.Equal(LOGIN, ((UsuarioJaExisteException)resposta.Erro).Login);
        }
    }
}
