namespace domain.core
{
    public interface IExecutar<Parametro, Resposta>
    {
        Resposta Executar(Parametro parametro);
    }
}
