using System;
using System.Collections.Generic;
using GIR.Core.Dados.Interface;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq.Expressions;

namespace GIR.Core.Dados.Infraestrutura
{
    public class RepositorioAtualizavel<TEntidade>
        : Repositorio<TEntidade>, IRepositorioAtualizavel<TEntidade> where TEntidade : class, IEntidade
    {
        public RepositorioAtualizavel(GIRContexto contexto) : base(contexto) { }

        public void AdicionarOuAtualizar(TEntidade entidade)
        {
            Set.AddOrUpdate(entidade);
        }

        public void AdicionarOuAtualizar(IEnumerable<TEntidade> entidades)
        {
            foreach (var entidade in entidades)
            {
                Set.AddOrUpdate(entidade);
            }
        }

        public void AdicionarOuAtualizar(Expression<Func<TEntidade, object>> criterio, params TEntidade[] entidade)
        {
            Set.AddOrUpdate(criterio, entidade);
        }

        public void AdicionarOuAtualizar(Expression<Func<TEntidade, object>> criterio, IEnumerable<TEntidade> entidades)
        {
            foreach (var entidade in entidades)
            {
                Set.AddOrUpdate(criterio, entidade);
            }
        }

        public void AdicionarRange(IEnumerable<TEntidade> entidades)
        {
             Set.AddRange(entidades);
        }

        public void Adicionar(TEntidade entidade)
        {
            Set.Add(entidade);
        }

        public void Atualizar(TEntidade entidade)
        {
            Contexto.Entry(entidade).State = EntityState.Modified;
        }

        public void Excluir(TEntidade entidade)
        {
            Set.Remove(entidade);
        }

        public void Excluir(int id)
        {
            var entidade = Obter(id);

            Set.Remove(entidade);
        }

        public void SalvarAlteracoes()
        {
            Contexto.SaveChanges();
        }
    }
}