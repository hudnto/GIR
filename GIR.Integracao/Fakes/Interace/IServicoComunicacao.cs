using GIR.Integracao.Model;

namespace GIR.Integracao.Fakes.Interace
{
    public interface IServicoComunicacao
    {
        void EnviarEmail(EstruturaEmail estruturaEmail);
    }
}
