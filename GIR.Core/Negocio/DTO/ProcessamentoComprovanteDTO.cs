using GIR.Core.Negocio.Enum;
using System.Collections.Generic;    

namespace GIR.Core.Negocio.DTO
{
    public class ProcessamentoComprovanteDTO
    {
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public int AnoExercicio { get; set; }
        public int? AnoCalendario { get; set; }
        public TipoContribuinte TipoContribuinte { get; set; }
        public IEnumerable<ComprovanteDTO> Comprovantes { get; set; }
        public TipoSituacao SituacaoProcessamento { get; set; }
        public ProcessamentoComprovanteDTO()
        {
            Comprovantes = new List<ComprovanteDTO>();
        }
    }
}
