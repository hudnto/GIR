using System;
using GIR.Core.Negocio.Consultas.Interface;
using GIR.Core.Negocio.Enum;

namespace GIR.Core.Negocio.Consultas.Filtro
{
    public class InformeFiltro : IFilter
    {
        public DateTime DataInicio { get; set; }
        public DateTime DataFinal { get; set; }
        public int AnoExercicio { get; set; }
        public TipoContribuinte TipoContribuinte { get; set; }
    }
}
