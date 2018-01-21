using System.Linq;
using GIR.Core.Dados;
using GIR.Core.Negocio.Consultas.Interface;
using GIR.Core.Negocio.DTO;
using GIR.Core.Negocio.Enum;
using SituacaoProcessamento = GIR.Core.Negocio.Enum.TipoSituacao;

namespace GIR.Core.Negocio.Consultas.QueryHandler
{
    public class ConsultaProcessamentoPeloIdQueryHandler : 
        IIn32QueryHandler<IQueryable<ProcessamentoComprovanteDTO>, int>
    {
        private readonly GIRContexto _db;

        public ConsultaProcessamentoPeloIdQueryHandler()
        {
            _db = new GIRContexto();
        }

        public IQueryable<ProcessamentoComprovanteDTO> Execute(int id)
        {
            var consulta = from p in _db.Processamentos
                           where p.Id == id
                           orderby p.DataRegistro
                           select new ProcessamentoComprovanteDTO
                           {
                               Codigo = p.Id,
                               AnoCalendario = p.AnoCalendario,
                               AnoExercicio = p.AnoExercicio,
                               Descricao = p.Descricao,
                               TipoContribuinte = TipoContribuinte.PrestadorFornecedor,
                               SituacaoProcessamento = TipoSituacao.Processado,
                               Comprovantes = from a in _db.Arquivos
                                              where a.ProcessamentoId == p.Id
                                              select new ComprovanteDTO{
                                                  CodigoProcessamento = a.ProcessamentoId,
                                                  DataUltimaAtualizacao = a.DataUltimaAtualizacao,
                                                  NomeArquivo = a.NomeArquivo                                                  
                                              }
                               
                           };

            return consulta;
        }

        public IQueryable<ProcessamentoDTO> Execute(int anoExercicio, TipoContribuinte tipo)
        {
            var consulta = from p in _db.Processamentos
                           let contribuintes = p.Contribuintes.FirstOrDefault()
                           where (contribuintes != null ? 
                                    p.AnoExercicio == anoExercicio && contribuintes.TipoContribuinteId == (short)tipo : 
                                    p.AnoExercicio == anoExercicio)  
                            select new ProcessamentoDTO()
                            {
                                Id = p.Id
                            }; 
            
            return consulta;
        }
    }
}
