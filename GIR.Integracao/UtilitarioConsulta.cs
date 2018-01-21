using GIR.Integracao.Model;
using GIR.Integracao.ServicoUtilitarioConsulta;
using System.Collections.Generic;
using System.Linq;

namespace GIR.Integracao
{
    public class UtilitarioConsulta : IUtilitarioConsulta
    {
        private readonly UtilitarioConsultaClient _servicoUtilitarioConsulta;

        public UtilitarioConsulta()
        {
            _servicoUtilitarioConsulta = new UtilitarioConsultaClient();
        }

        public Estado[] BuscarUfs()
        {
            return _servicoUtilitarioConsulta.listarEstados();
        }

        public IEnumerable<Uf> ListarUfs()
        {
            var response = _servicoUtilitarioConsulta.listarEstados();
            return response.Select(r => new Uf { Id = r.idEstado, Nome = r.nome, Sigla = r.sigla }).ToList();
        }

        public Uf ObterUfPorId(int uf)
        {
            var response = _servicoUtilitarioConsulta.obterEstado(uf);
            return new Uf() { Id = response.idEstado, Nome = response.nome, Sigla = response.sigla };
        }
    }
}
