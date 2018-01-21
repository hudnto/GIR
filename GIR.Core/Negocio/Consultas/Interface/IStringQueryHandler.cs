namespace GIR.Core.Negocio.Consultas.Interface
{
    /// <summary>
    /// Retringe para consultas que tem como parâmetro o tipo string
    /// </summary>
    /// <typeparam name="TResult">Retorno da consulta</typeparam
    public interface IStringQueryHandler<out TResult, in TFilter> 
    {
        TResult Execute(string filtro);
    }
}

