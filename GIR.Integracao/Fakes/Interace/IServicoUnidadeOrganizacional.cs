using GIR.Integracao.ServicoUnidadeOrganizacional;

namespace GIR.Integracao.Fakes.Interace
{
    public interface IServicoUnidadeOrganizacional
    {
        UnidadeOrganizacional ObterUnidadeOrganizacional(int codigoUnidade);
    }
}
