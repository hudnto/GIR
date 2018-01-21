using System.Collections.Generic;
using GIR.Integracao.Model;

namespace GIR.Integracao.Fakes.Interace
{
    public interface IServicoGasc
    {
        ICollection<PerfilUsuario> RecuperarPerfisUsuario(int usuarioId);
    }
}
