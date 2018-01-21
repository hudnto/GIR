using System.ComponentModel.DataAnnotations;
using GIR.Core.Negocio.Consultas.Filtro;
using GIR.Core.Negocio.Mensagem;

namespace GIR.Intranet.Models
{
    public class ConsultaInformeColaboradorVM
    {
        [Display(Name = "Ano Base")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MA001")]
        public int AnoExercicio { get; set; }

        [Display(Name = "CPF/CNPJ")]
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MA001")]
        public string CPFCNPJ { get; set; }

        public static InformeColaboradorFiltro Converter(ConsultaInformeColaboradorVM vm)
        {
            var filtro = new InformeColaboradorFiltro()
            {
                AnoExercicio = vm.AnoExercicio,
                CPFCNPJ = removeMascaraCPFCNPJ(vm.CPFCNPJ)
            };

            return filtro;
        }

        /// <summary>
        /// Remove os caracteres não numéricos do CPF/CNPJ
        /// </summary>
        /// <param name="cpfCnpj">CPF/CNPJ com máscara</param>
        /// <returns>Sytem.String</returns>
        static string removeMascaraCPFCNPJ(string cpfCnpj){

            if (cpfCnpj.Contains("."))
            {
                cpfCnpj = cpfCnpj.Replace(".", "");
            }

            if (cpfCnpj.Contains("-"))
            {
                cpfCnpj = cpfCnpj.Replace("-", "");
            }

            if (cpfCnpj.Contains("/"))
            {
                cpfCnpj = cpfCnpj.Replace("/", "");
            }

            return cpfCnpj;
        }
    }
}