using GIR.Core.Negocio.Consultas.Interface;
using GIR.Core.Negocio.Enum;

namespace GIR.Core.Negocio.Consultas.Filtro
{
    public class ProcessamentoFiltro : IFilter
    {
        public int AnoExercicio { get; set; }
        public TipoContribuinte TipoContribuinte { get; set; }
    }
}
