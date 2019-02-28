using entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace domain.response
{
    public class InserirUsuarioResponse
    {
        public Usuario Usuario { get; internal set; }

        public bool IsInserido { get; internal set; }

        public Exception Erro { get; internal set; }

        public InserirUsuarioResponse(Usuario usuario)
        {
            Usuario = usuario;
            IsInserido = true;
            Erro = null;
        }

        public InserirUsuarioResponse(Exception erro)
        {
            Usuario = null;
            IsInserido = false;
            Erro = erro;
        }
    }
}
