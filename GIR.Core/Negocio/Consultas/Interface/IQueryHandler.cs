namespace GIR.Core.Negocio.Consultas.Interface
{
    public interface IQueryHandler<out TResult, in TFilter> where TFilter : IFilter
    {
        TResult Execute(TFilter filtro);
    }
}

