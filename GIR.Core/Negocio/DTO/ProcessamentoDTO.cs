using System;
using System.Collections.Generic;
using GIR.Core.Negocio.Consultas.Filtro;
using GIR.Core.Negocio.Enum;

namespace GIR.Core.Negocio.DTO
{
    public class ProcessamentoDTO
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public int AnoExercicio { get; set; }
        public int AnoCalendario { get; set; }
        public DateTime DataRegistro { get; set; }
        public bool Individual { get; set; }
        public string LoginUsuario { get; set; }
        public TipoContribuinte? TipoContribuinte { get; set; }
        public TipoSituacao TipoSituacao { get; set; }
        public int TotalArquivosGerados { get; set; }
        public IEnumerable<ArquivoDTO> ArquivosImportados { get; set; }
        public IEnumerable<String> Ocorrencias { get; set; }

        public ProcessamentoDTO()
        {
            Ocorrencias = new List<String>();
        }

        public static ProcessamentoFiltro Converter(int anoExercicio, TipoContribuinte tipo)
        {
            var filtro = new ProcessamentoFiltro()
            {
                AnoExercicio = anoExercicio,
                TipoContribuinte = tipo
            };

            return filtro;
        }
    }
}
