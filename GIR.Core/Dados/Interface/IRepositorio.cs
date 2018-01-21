using System;
using System.Linq;
using System.Linq.Expressions;

namespace GIR.Core.Dados.Interface
{
    public interface IRepositorio<TEntidade> where TEntidade : class, IEntidade
    {
        TEntidade Obter(int id);

        TEntidade Obter(Expression<Func<TEntidade, bool>> criterio);

        IQueryable<TEntidade> Buscar(Expression<Func<TEntidade, bool>> criterio, Func<TEntidade, object> ordenarPor, params Expression<Func<TEntidade, object>>[] propriedadesDeNavegacao);

        IQueryable<TEntidade> Todos();

        bool Existe(int id);

        bool Existe(Expression<Func<TEntidade, bool>> criterio);

        int Contar(Expression<Func<TEntidade, bool>> criterio);
    }
}