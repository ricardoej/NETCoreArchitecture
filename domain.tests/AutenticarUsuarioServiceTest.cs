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
    public class AutenticarUsuarioServiceTest
    {
        private readonly AutenticarUsuarioService service;

        public AutenticarUsuarioServiceTest()
        {
            var repositoryMock = Substitute.For<IUsuarioRepository>();
            repositoryMock.BuscarPorLogin("ricardo").Returns(new Usuario()
            {
                Id = 1,
                Login = "ricardo",
                Email = "ricardo@mail.com",
                Nome = "Ricardo",
                SenhaHash = new byte[64] { 88, 141, 53, 147, 195, 221, 120, 76, 133, 58, 75, 245, 154, 66, 171, 36, 195, 218, 184, 30, 251, 4, 146, 2, 74, 28, 190, 46, 240, 197, 64, 240, 33, 106, 77, 229, 80, 236, 72, 165, 33, 137, 44, 164, 193, 126, 233, 148, 157, 146, 39, 168, 219, 240, 199, 150, 153, 185, 26, 243, 106, 165, 54, 54 },
                SenhaSalt = new byte[128] { 9, 69, 222, 255, 109, 36, 87, 75, 120, 46, 116, 133, 76, 189, 194, 33, 227, 10, 48, 7, 60, 38, 59, 250, 154, 170, 230, 99, 122, 172, 45, 61, 167, 171, 158, 209, 239, 73, 208, 95, 65, 119, 127, 70, 30, 104, 235, 48, 233, 248, 103, 20, 71, 196, 32, 160, 255, 205, 82, 36, 145, 105, 43, 66, 155, 38, 84, 44, 73, 147, 39, 235, 5, 235, 133, 114, 69, 228, 172, 147, 1, 8, 14, 55, 133, 43, 140, 187, 60, 103, 72, 28, 145, 69, 66, 122, 93, 10, 53, 204, 151, 219, 103, 156, 87, 60, 234, 160, 171, 83, 227, 171, 42, 157, 18, 30, 69, 61, 109, 93, 36, 135, 108, 110, 242, 119, 217, 83 }
            });
            service = new AutenticarUsuarioService(repositoryMock);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void RetornaExceçãoLoginNullOrEmBranco(string login)
        {
            var parametro = new AutenticarRequest()
            {
                Login = login,
                Senha = "123"
            };

            var resposta = service.Executar(parametro);

            Assert.False(resposta.IsLogado);
            Assert.IsType<ArgumentNullException>(resposta.Erro);
            Assert.Equal("Login", ((ArgumentNullException)resposta.Erro).ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void RetornaExceçãoSenhaNullOrEmBranco(string senha)
        {
            var parametro = new AutenticarRequest()
            {
                Login = "ricardo",
                Senha = senha
            };

            var resposta = service.Executar(parametro);

            Assert.False(resposta.IsLogado);
            Assert.IsType<ArgumentNullException>(resposta.Erro);
            Assert.Equal("Senha", ((ArgumentNullException)resposta.Erro).ParamName);
        }

        [Fact]
        public void RetornaExceçãoUsuarioNaoEncontrado()
        {
            var parametro = new AutenticarRequest()
            {
                Login = "joao",
                Senha = "123"
            };

            var resposta = service.Executar(parametro);

            Assert.False(resposta.IsLogado);
            Assert.IsType<UsuarioNaoEncontradoException>(resposta.Erro);
        }

        [Fact]
        public void RetornaExceçãoSenhaIncorreta()
        {
            var parametro = new AutenticarRequest()
            {
                Login = "ricardo",
                Senha = "321"
            };

            var resposta = service.Executar(parametro);

            Assert.False(resposta.IsLogado);
            Assert.IsType<SenhaIncorretaException>(resposta.Erro);
        }

        [Fact]
        public void RetornaSucessoUsuarioLogado()
        {
            var parametro = new AutenticarRequest()
            {
                Login = "ricardo",
                Senha = "123"
            };

            var resposta = service.Executar(parametro);

            Assert.True(resposta.IsLogado);
            Assert.Equal(1, resposta.Usuario.Id);
        }
    }
}
