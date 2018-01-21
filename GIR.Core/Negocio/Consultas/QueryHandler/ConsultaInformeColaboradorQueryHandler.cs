using System.Linq;
using GIR.Core.Dados;
using GIR.Core.Negocio.Consultas.Filtro;
using GIR.Core.Negocio.Consultas.Interface;
using GIR.Core.Negocio.DTO;
using GIR.Core.Negocio.Enum;

namespace GIR.Core.Negocio.Consultas.QueryHandler
{
    public class ConsultaInformeColaboradorQueryHandler : 
        IQueryHandler<IQueryable<InformeColaboradorDTO>, InformeColaboradorFiltro>
    {
        private readonly GIRContexto _db;

        public ConsultaInformeColaboradorQueryHandler()
        {
            _db = new GIRContexto();
        }

        public IQueryable<InformeColaboradorDTO> Execute(InformeColaboradorFiltro filtro)
        {
            var consulta = from p in _db.Processamentos
                           join a in _db.Arquivos
                                on p.Id equals a.ProcessamentoId
                           where p.AnoExercicio == filtro.AnoExercicio && p.SituacaoProcessamento == (short)TipoSituacao.EnviadoWeb &&
                               p.Arquivos.Any(arquivo => a.NomeArquivo.Contains(filtro.CPFCNPJ))
                           orderby p.AnoExercicio
                           select new InformeColaboradorDTO
                           {
                               AnoExercicio = p.AnoExercicio,
                               CPFCNPJ = a.NomeArquivo,
                               CaminhoDiretorio = a.CaminhoDiretorio,
                               ExtensaoArquivo = a.ExtensaoArquivo
                           };

            return consulta;
        }



    }
}
