using System;
using System.Collections.Generic;
using System.Text;

namespace data.request
{
    public class UsuarioRequest
    {
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}
