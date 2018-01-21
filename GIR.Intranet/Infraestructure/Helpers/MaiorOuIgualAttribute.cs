using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace GIR.Intranet.Infraestructure.Helpers
{
    public class MaiorOuIgualAttribute : ValidationAttribute
    {
        public string PropriedadeParaComparar { get; set; }

        protected override ValidationResult IsValid(object valorPassado, ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(PropriedadeParaComparar)) throw new ArgumentNullException(PropriedadeParaComparar);

            var propriedadeParaComparacao = validationContext.ObjectInstance.GetType()
                                                                            .GetProperties()
                                                                            .AsQueryable()
                                                                            .Where(f => f.Name == PropriedadeParaComparar);

            if (!propriedadeParaComparacao.Any())
                throw new Exception("Propriedade inválida. Propriedade não encontrada");
            if (propriedadeParaComparacao.Count() > 1)
                throw new Exception("Propriedade inválida. Várias propriedades com o mesmo nome encontrada");
            if (valorPassado.GetType() != propriedadeParaComparacao.First().PropertyType)
                throw new Exception("Propriedade inválida. Propriedades não são do mesmo tipo");

            var valorParaComparar = propriedadeParaComparacao.First().GetValue(validationContext.ObjectInstance, BindingFlags.Public, null, null, null) as IComparable;
            var isValido = valorParaComparar != null && valorParaComparar.CompareTo(valorPassado) <= 0;

            return isValido ? null : new ValidationResult(ErrorMessageString, new[] { PropriedadeParaComparar});
        }


    }
}