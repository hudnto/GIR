using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GIR.Intranet.Infraestructure.Extensions
{
    public static class ModelStateExtension
    {
        public static object ObterErroModelState(this ModelStateDictionary modelState)
        {
            var chaves = from modelstate in modelState.AsQueryable().Where(f => f.Value.Errors.Count > 0)
                         select modelstate.Key;
            var listaErros = from modelstate in modelState.AsQueryable().Where(f => f.Value.Errors.Count > 0)
                             select modelstate.Value.Errors.FirstOrDefault(a => !String.IsNullOrEmpty(a.ErrorMessage));

            var resultado = new { sucesso = false, chaves = chaves.ToList(), listaErros = listaErros.Where(a => a != null).Select(a => a.ErrorMessage).ToList() };

            return resultado;
        }
    }
}