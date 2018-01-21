using System;
using System.Collections.Generic;
using System.Linq;
using Cassi.Utilitarios.Util;
using GIR.Core.Dados;
using GIR.Core.Dados.Entidade;
using GIR.Core.Infraestrutura.Extensoes;
using GIR.Core.Negocio.Enum;
using GIR.Core.Negocio.Servico;

namespace GIR.Core.Negocio.DTO
{
    public class LoteDTO
    {
        public int Codigo { get; set; }
        public String Descricao { get; set; }
        public int AnoExercicio { get; set; }
        public int AnoCalendario { get; set; }
        public DateTime InicioProcessamento { get; set; }
        public TipoContribuinte TipoContribuinte { get; set; }
        public TipoSituacao TipoSituacao { get; set; }
        public OperacaoGIR Operacao { get; set; }
        public IEnumerable<ArquivoDTO> ArquivosImportados { get; set; }
        public int TotalArquivosImportados { get; set; }
        public int TotalArquivosGerados { get; set; }
        public List<ContribuinteDTO> ContribuintesArquivoDirf { get; set; }
        public bool Individual { get; set; }
        public string LoginUsuario { get; set; }
        public String Ocorrencias { get; set; }
        public int CodigoErro { get; set; }
        public string MensagemErro { get; set; }

        public LoteDTO()
        {
            ArquivosImportados = new List<ArquivoDTO>();
            ContribuintesArquivoDirf = new List<ContribuinteDTO>();
        }
    }
}