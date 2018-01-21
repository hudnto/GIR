namespace GIR.Core.Dados.Interface
{
    public interface IRepositorioAtualizavel<TEntidade> : IRepositorio<TEntidade>
        where TEntidade : class, IEntidade
    {
        void Adicionar(TEntidade entidade);

        void Atualizar(TEntidade entidade);

        void Excluir(TEntidade entidade);

        void Excluir(int entidade);

        void SalvarAlteracoes();
    }
}