using GIR.Core.Negocio.Enum;
using GIR.Core.Negocio.DTO;
using System.Collections.Generic;
using System.ComponentModel;

namespace GIR.Intranet.Models
{
    public class VisualizaComprovanteVM
    {
        [DisplayName(@"Código:")]
        public int Codigo { get; set; }

        [DisplayName(@"Descrição:")]
        public string Descricao { get; set; }

        [DisplayName(@"Ano Exercício:")]
        public int AnoExercicio { get; set; }

        [DisplayName(@"Ano Calendário:")]
        public int AnoCalendario { get; set; }

        [DisplayName(@"CPF/CNPJ")]
        public string CpfCnpj { get; set; }

        [DisplayName(@"UF")]
        public string Uf { get; set; }

        [DisplayName(@"Unid. Org.:")]
        public string UnidadeOrganizacional { get; set; }

        [DisplayName(@"Descr. Unid. Org.:")]
        public string UnidadeOrganizacionalDescr { get; set; }

        public TipoSituacao SituacaoProcessamento { get; set; }

        public TipoContribuinte TipoContribuinte { get; set; }

        public IEnumerable<ComprovanteVM> Comprovantes { get; set; }

        public VisualizaComprovanteVM()
        {
            Comprovantes = new List<ComprovanteVM>();
        }

        public static VisualizaComprovanteVM Converter(ProcessamentoComprovanteDTO dto)
        {
            var vm = new VisualizaComprovanteVM
            {
                Codigo = dto.Codigo,
                AnoCalendario = dto.AnoCalendario ?? 0,
                AnoExercicio = dto.AnoExercicio,
                Descricao = dto.Descricao,
                SituacaoProcessamento = dto.SituacaoProcessamento,
                TipoContribuinte = dto.TipoContribuinte,
                Comprovantes = ComprovanteVM.Converter(dto.Comprovantes)
            };

            return vm;
        }

    }
}