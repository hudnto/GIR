using System;
using System.Text.RegularExpressions;
using Cassi.Utilitarios.Util;

namespace GIR.Intranet.Infraestructure.Extensions
{
    public static class UtilExtension
    {
        public static string MascaraData(this DateTime? data)
        {
            return data.HasValue ? data.Value.ToShortDateString() : String.Empty;
        }

        public static string FormatarCnpj(this string cnpj)
        {
            var formatado = FormatacaoUtil.FormatarCNPJ(cnpj);
            return formatado;
        }

        /// <summary>
        ///  Recebe um CNPJ ou CPF e retorna a string sem os separadores
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static string RemoverMascaraCpFouCnpj(this string valor)
        {
            return valor == null ? null : Regex.Replace(valor, @"[^\d]", "");
        }

        /// <summary>
        /// Reduz a string para o tamanho de caractes difinido no parâmetro.
        /// </summary>
        /// <param name="valor"></param>
        /// <param name="comprimento">Comprimento</param>
        /// <param name="apresentarReticencias"></param>
        /// <returns>String com o tamanho definido pelo parâmetro</returns>
        public static string Reduzir(this string valor, int comprimento, bool apresentarReticencias = true)
        {
            if (string.IsNullOrEmpty(valor) || valor.Length < comprimento)
            {
                return valor;
            }
            var textoReduzido = valor.Substring(0, comprimento);

            if (apresentarReticencias)
                textoReduzido = string.Concat(textoReduzido, " (...)");

            return textoReduzido;
        } 
        
    }
}