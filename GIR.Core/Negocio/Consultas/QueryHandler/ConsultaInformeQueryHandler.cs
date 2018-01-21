using System.Linq;
using GIR.Core.Dados;
using GIR.Core.Negocio.Consultas.Filtro;
using GIR.Core.Negocio.Consultas.Interface;
using GIR.Core.Negocio.DTO;
using GIR.Core.Negocio.Enum;

namespace GIR.Core.Negocio.Consultas.QueryHandler
{
    public class ConsultaInformeQueryHandler : IQueryHandler<IQueryable<InformeRendimentoDTO>, InformeFiltro>
    {
        private readonly GIRContexto _db;

        public ConsultaInformeQueryHandler()
        {
            _db = new GIRContexto();
        }

        public IQueryable<InformeRendimentoDTO> Execute(InformeFiltro filtro)
        {
            var consulta = from p in _db.Processamentos
                           where p.DataRegistro >= filtro.DataInicio && p.DataRegistro <= filtro.DataFinal
                           orderby p.Id
                           select new InformeRendimentoDTO()
                           {
                               Codigo = p.Id,
                               Ano = p.AnoExercicio,
                               Data = p.DataRegistro,
                               Descricao = p.Descricao,
                               IdSituacao = p.SituacaoProcessamento,
                               Contribuinte = new ContribuinteDTO()
                               {
                                    IdTipo = p.Contribuintes.FirstOrDefault().TipoContribuinteId
                               }
                           };

            if (filtro.AnoExercicio > 0)
                consulta = consulta.Where(c => c.Ano == filtro.AnoExercicio);

            if (filtro.TipoContribuinte != TipoContribuinte.Todos)
                consulta = consulta.Where(c => c.Contribuinte.IdTipo == (short)filtro.TipoContribuinte);

            return consulta;
        }
    }
}
