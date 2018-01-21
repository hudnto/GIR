using System;
using System.Linq;
using GIR.Core.Dados;
using GIR.Core.Negocio.Consultas.Filtro;
using GIR.Core.Negocio.Consultas.Interface;
using GIR.Core.Negocio.DTO;

namespace GIR.Core.Negocio.Consultas.QueryHandler
{
    public class ConsultaComprovanteRendimentoDirfQueryHandler :
        IQueryHandler<IQueryable<ComprovanteDTO>, ComprovanteRendimentoDirfFiltro>
    {
        private readonly GIRContexto _db;

        public ConsultaComprovanteRendimentoDirfQueryHandler()
        {
            _db = new GIRContexto();
        }

        public IQueryable<ComprovanteDTO> Execute(ComprovanteRendimentoDirfFiltro filtro)
        {
            var resultado = from a in _db.Arquivos
                            from c in a.Processamento.Contribuintes
                            where a.ProcessamentoId == filtro.CodigoProcessamento &&
                                  c.CPFCNPJ == filtro.CpfCnpj ||
                                  c.EstadoId == filtro.UfId ||
                                  c.UnidadeOrganizacional == filtro.UnidadeOrganzacionalId
                            select new ComprovanteDTO
                            {
                                CodigoProcessamento = a.ProcessamentoId,
                                DataUltimaAtualizacao = a.DataUltimaAtualizacao,
                                NomeArquivo = a.NomeArquivo,
                                UnidadeOrganizacionalId = c.UnidadeOrganizacional,
                                UnidadeOrganizacional = c.UnidadeOrganizacionalDescr,
                                Email = c.Email,
                                CpfCnpj = c.CPFCNPJ,
                                Uf = Convert.ToInt16(c.EstadoId)
                            };

            return resultado;
        }
    }
}
