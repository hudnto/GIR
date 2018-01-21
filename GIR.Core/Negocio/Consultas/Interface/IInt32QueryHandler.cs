using System;
namespace GIR.Core.Negocio.Consultas.Interface
{
    /// <summary>
    /// Retringe para consultas que tem como parâmetro o tipo int
    /// </summary>
    /// <typeparam name="TResult">Retorno da consulta</typeparam
    public interface IIn32QueryHandler<out TResult, in TFilter> 
    {
        TResult Execute(int filtro);
    }
}

