namespace GIR.Core.Infraestrutura.Consulta
{
    public interface IQueryHandler<out TResult, in TFilter> : IFilter
    {
        TResult Execute(TFilter filtro);
    }
}
