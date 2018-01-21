using System.Collections.Generic;
using GIR.Integracao.Fakes.Interace;
using GIR.Integracao.ServicoGASC;
using PerfilUsuario = GIR.Integracao.Model.PerfilUsuario;

namespace GIR.Integracao.Fakes
{
    public class ServicoGasc : IServicoGasc
    {
        private readonly ServicoGASCClient _servicoGasc;

        public ServicoGasc()
        {
            _servicoGasc = new ServicoGASCClient();
        }

        public ICollection<PerfilUsuario> RecuperarPerfisUsuario(int usuarioId)
        {
            ICollection<PerfilUsuario> perfis = null;

            var retorno = _servicoGasc.ListarPerfisUsuario(usuarioId);

            if (retorno.Codigo == 0)
                perfis = PerfilUsuario.Converter(retorno.Perfis);

            return perfis;
        }
    }
}
