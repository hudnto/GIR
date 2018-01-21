using GIR.Core.Dados.Interface;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace GIR.Core.Dados.Infraestrutura
{
    public class Repositorio<TEntidade> : IRepositorio<TEntidade> where TEntidade : class, IEntidade
    {
        protected GIRContexto Contexto;
        protected DbSet<TEntidade> Set;

        public Repositorio()
        {
            Contexto = new GIRContexto();
            Set = Contexto.Set<TEntidade>();
        }

        public Repositorio(GIRContexto contexto)
        {
            Contexto = contexto;
            Set = Contexto.Set<TEntidade>();
        }

        public virtual TEntidade Obter(int id)
        {
            return Set.Find(id);
        }

        public virtual TEntidade Obter(Expression<Func<TEntidade, bool>> criterio)
        {
            return Set.Where(criterio).FirstOrDefault();
        }

        public virtual TEntidade ObterUltimo(Expression<Func<TEntidade, bool>> criterio)
        {
            return Set.Where(criterio).OrderByDescending(a => a.Id).FirstOrDefault();
        }

        public IQueryable<TEntidade> Buscar(Expression<Func<TEntidade, bool>> criterio,
            Func<TEntidade, object> ordenarPor = null,
            params Expression<Func<TEntidade, object>>[] propriedadesDeNavegacao)
        {
            IQueryable<TEntidade> set = Set;

            IncluirPropriedadesNaQuery(propriedadesDeNavegacao, set);

            IQueryable<TEntidade> resultado = ordenarPor != null ?
                set.Where(criterio).OrderBy(ordenarPor).AsQueryable() :
                set.Where(criterio).AsQueryable();


            return resultado;
        }

        public IQueryable<TEntidade> Todos()
        {
            return Set;
        }

        public bool Existe(int id)
        {
            return Set.Any(a => a.Id == id);
        }

        public bool Existe(Expression<Func<TEntidade, bool>> criterio)
        {
            return Set.Any(criterio);
        }

        public int Contar(Expression<Func<TEntidade, bool>> criterio)
        {
            return Set.Count(criterio);
        }

        private void IncluirPropriedadesNaQuery(Expression<Func<TEntidade, object>>[] propriedadesDeNavegacao, IQueryable<TEntidade> entidade)
        {
            propriedadesDeNavegacao.Aggregate(entidade, (current, propriedade) => current.Include(propriedade));
        }
    }
}