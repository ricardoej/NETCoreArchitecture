using entities;
using System;

namespace domain.response
{
    public class AutenticarResponse
    {
        public Usuario Usuario { get; internal set; }

        public bool IsLogado { get; internal set; }

        public Exception Erro { get; internal set; }

        public AutenticarResponse(Usuario usuario)
        {
            Usuario = usuario;
            IsLogado = true;
            Erro = null;
        }

        public AutenticarResponse(Exception erro)
        {
            Usuario = null;
            IsLogado = false;
            Erro = erro;
        }
    }
}
