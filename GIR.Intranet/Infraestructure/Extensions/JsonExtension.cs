using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GIR.Core.Negocio.DTO;
using GIR.Intranet.Models;

namespace GIR.Intranet.Infraestructure.Extensions
{
    public static class JsonExtension
    {
        private static IEnumerable<string> listaCamposExcluidos = new List<string> { "X-Requested-With", "__RequestVerificationToken" };

        public static object FormToJson(this FormCollection form, bool mostrarCamposNulos)
        {
            if (form.HasKeys())
            {
                var resultado = new Dictionary<string, string>();
                foreach (var campo in form.AllKeys)
                {
                    if (!listaCamposExcluidos.Contains(campo)
                                && String.IsNullOrEmpty(form[campo]) == mostrarCamposNulos)
                    {
                        resultado.Add(campo, form[campo]);
                    }
                }
                return resultado;
            }

            return null;
        }

        public static IEnumerable<object> ParseParaJson(this IEnumerable<ArquivoVM> arquivos)
        {
            var resultado = arquivos.Select(a => new
            {
                Id = 0,
                Tamanho = 12,
                Nome = a.NomeArquivo,
                FalhaAoImportar = a.ArquivoFalhouAoImportar
            }).ToList();

            return resultado;
        }
    }
}