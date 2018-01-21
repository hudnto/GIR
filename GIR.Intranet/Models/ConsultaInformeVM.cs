using GIR.Core.Negocio.Mensagem;
using GIR.Intranet.Infraestructure.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using GIR.Core.Negocio.Consultas.Filtro;
using GIR.Core.Negocio.Enum;

namespace GIR.Intranet.Models
{
    public class ConsultaInformeVM
    {
        [Display(Name = "Inicial")]
        [Required(ErrorMessageResourceName = "MA001", ErrorMessageResourceType = typeof(Mensagem))]
        public DateTime DataInicio { get; set; }

        [Display(Name = "Final")]
        [Required(ErrorMessageResourceName = "MA001", ErrorMessageResourceType = typeof(Mensagem))]
        [MaiorOuIgual(PropriedadeParaComparar = "DataInicio", ErrorMessageResourceName = "MA003", ErrorMessageResourceType = typeof(Mensagem))]
        public DateTime DataFinal { get; set; }

        [Display(Name = "Ano Exercício")]
        public int AnoExercicio { get; set; }

        public TipoContribuinte TipoContribuinte { get; set; }

        public static InformeFiltro Converter(ConsultaInformeVM vm)
        {
            var filtro = new InformeFiltro()
            {
                DataInicio = vm.DataInicio,
                DataFinal = vm.DataFinal.AddSeconds(86399),
                AnoExercicio = vm.AnoExercicio,
                TipoContribuinte = vm.TipoContribuinte
            };

            return filtro;
        }
    }
}