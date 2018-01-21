using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GIR.Intranet.Infraestructure.Extensions
{
    public static class ValidationResultExtension
    {
        // Data de criação da CASSI - Aqui é cultura também!
        private const int AnoMinimo = 1944;
        // Futuro improvável 
        private const int AnoMaximo = 2100;
        public static void ValidarData(this List<ValidationResult> lista, DateTime? dataInicial, DateTime? dataFinal,
                                        string nomeInicial, string nomeFinal)
        {
            //var mensagemJaPassada = false;
            //if (dataInicial.HasValue && dataInicial.Value.Year < AnoMinimo)
            //{
            //    lista.Add(new ValidationResult(Mensagens.MA022, new List<string> { nomeInicial }));
            //    mensagemJaPassada = true;
            //}
            //if (dataFinal.HasValue && dataFinal.Value.Year > AnoMaximo)
            //{
            //    lista.Add(new ValidationResult(mensagemJaPassada ? string.Empty : Mensagens.MA022, new List<string> { nomeFinal }));
            //}
        }
    }
}
