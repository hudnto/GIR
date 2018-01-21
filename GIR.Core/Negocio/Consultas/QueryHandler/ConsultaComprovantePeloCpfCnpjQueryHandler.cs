using System;
using System.Linq;
using GIR.Core.Dados;
using GIR.Core.Negocio.Consultas.Filtro;
using GIR.Core.Negocio.Consultas.Interface;
using GIR.Core.Negocio.DTO;

namespace GIR.Core.Negocio.Consultas.QueryHandler
{
    public class ConsultaComprovantePeloCpfCnpjQueryHandler : IQueryHandler<IQueryable<ArquivoDTO>, ConferirComprovanteFiltro>
    {
        private readonly GIRContexto _contexto;

        public ConsultaComprovantePeloCpfCnpjQueryHandler()
        {
            _contexto = new GIRContexto();
        }

        public IQueryable<ArquivoDTO> Execute(ConferirComprovanteFiltro filtro)
        {
            var consulta = _contexto.Arquivos.AsQueryable();

            if (filtro != null)
            {
                consulta = consulta.Where(m => m.NomeArquivo.Contains(filtro.CpfCnpj));
                consulta = consulta.Where(m => m.NomeArquivo.Contains(filtro.AreaRh));
                consulta = consulta.Where(m => m.NomeArquivo.Contains(filtro.Uf));
            }  

            var resultado = from a in consulta
                            where a.Id == 0
                            orderby a.Id
                            select new ArquivoDTO()
                                {
                                    CaminhoArquivo = a.CaminhoDiretorio,
                                    DataUltimaAtualizacao = a.DataUltimaAtualizacao,
                                    ExtensaoArquivo = a.ExtensaoArquivo,
                                    ArquivoId = a.Id,
                                    NomeArquivo = a.NomeArquivo,
                                    SituacaoId = a.SituacaoId
                                };

            return resultado;
        }
    }
}
