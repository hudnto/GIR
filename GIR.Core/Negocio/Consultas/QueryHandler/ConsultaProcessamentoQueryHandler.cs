using System;
using System.Linq;
using GIR.Core.Dados;
using GIR.Core.Negocio.Consultas.Filtro;
using GIR.Core.Negocio.Consultas.Interface;
using GIR.Core.Negocio.DTO;
using GIR.Core.Negocio.Enum;
using SituacaoProcessamento = GIR.Core.Negocio.Enum.TipoSituacao;

namespace GIR.Core.Negocio.Consultas.QueryHandler
{
    public class ConsultaProcessamentoQueryHandler :
        IQueryHandler<IQueryable<ProcessamentoDTO>, ProcessamentoFiltro>
    {
        private readonly GIRContexto _db;

        public ConsultaProcessamentoQueryHandler()
        {
            _db = new GIRContexto();
        }

        public IQueryable<ProcessamentoDTO> Execute(ProcessamentoFiltro filtro)
        {
            var consulta =
                _db.Contribuintes.Where(c => c.Processamentos.Any(p => p.AnoExercicio == filtro.AnoExercicio) && c.TipoContribuinteId == (short)filtro.TipoContribuinte)
                    .Select(pr => new ProcessamentoDTO()
                    {
                        Id = pr.Processamentos.FirstOrDefault(p => p.AnoExercicio == filtro.AnoExercicio).Id,
                        TipoContribuinte = (TipoContribuinte)pr.TipoContribuinteId
                    });

            if (consulta.FirstOrDefault() != null)
                return consulta;

            var query =
                _db.Processamentos.Where(p => (p.AnoExercicio == filtro.AnoExercicio && p.Contribuintes.Count == 0))
                    .Select(pr => new ProcessamentoDTO()
                    {
                        Id = pr.Id,
                        TipoContribuinte = null
                    });

            return query;

        }

        public IQueryable<ProcessamentoDTO> Execute(int id)
        {
            var consulta = _db.Processamentos.Where(p => p.Id == id).Select(p => new ProcessamentoDTO()
            {
                Id = p.Id,
                AnoExercicio = p.AnoExercicio,
                AnoCalendario = p.AnoCalendario,
                Descricao = p.Descricao,
                Individual = p.Individual,
                TipoSituacao = (TipoSituacao)p.SituacaoProcessamento,
                DataRegistro = p.DataRegistro,
                TipoContribuinte = (p.Contribuintes.Any() ? (TipoContribuinte)p.Contribuintes.FirstOrDefault().TipoContribuinteId : TipoContribuinte.Nenhum),
                TotalArquivosGerados = (p.Arquivos.Count(c => c.CaminhoDiretorio.Contains("PROCESSADOS")) +
                                        p.Arquivos.Count(c => c.CaminhoDiretorio.Contains("WEB"))),
                ArquivosImportados = p.Arquivos.Where(a => a.CaminhoDiretorio.Contains("IMPORTADOS")).Select(a => new ArquivoDTO()
                {
                    NomeArquivo = a.NomeArquivo,
                    ExtensaoArquivo = a.ExtensaoArquivo
                }),
                Ocorrencias = p.Ocorrencias.Where(o => o.DescricaoOcorrencia.Contains("Erro")).OrderBy(o => o.Id).Select(o => o.DescricaoOcorrencia),
                LoginUsuario = p.Ocorrencias.FirstOrDefault().LoginUsuario
            });

            return consulta;
        }
    }
}
