using System;
using System.Collections.Generic;
using System.Text;

namespace domain.exceptions
{
    public class UsuarioJaExisteException: Exception
    {
        public string Login { get; set; }

        public UsuarioJaExisteException(string login)
        {
            Login = login;
        }
    }
}
