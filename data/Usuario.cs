using data.core;

namespace data
{
    public class Usuario: BaseEntity
    {
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        
    }
}