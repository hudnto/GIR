using GIR.Integracao.Model;

namespace GIR.Integracao.Fakes.Interace
{
    public interface IServicoColaborador
    {
        Colaborador ObterColaborador(string loginColaborador);
    }
}
