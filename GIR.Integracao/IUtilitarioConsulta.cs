using System.Collections.Generic;
using GIR.Integracao.Model;

namespace GIR.Integracao
{
    public interface IUtilitarioConsulta
    {
        IEnumerable<Uf> ListarUfs();
        Uf ObterUfPorId(int ufId);
    }
}
