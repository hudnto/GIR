using System;
using GIR.Core.Negocio.Consultas.Interface;

namespace GIR.Core.Negocio.Consultas.Filtro
{
    public class ConferirComprovanteFiltro : IFilter
    {
        public string CpfCnpj { get; set; }
        public string Uf { get; set; }
        public string AreaRh { get; set; }
        public DateTime? AnoExercicio { get; set; }
        public DateTime? AnoCalendario { get; set; }
    }
}
