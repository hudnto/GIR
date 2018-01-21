using System.Web.Mvc;
using FluentValidation;
using AppLogger;
using GIR.Core.Negocio.Erro;
using GIR.Intranet.Infraestructure.Extensions;

namespace GIR.Intranet.Infraestructure.Filters
{
    public class GlobalErrorHandlerAttribute : HandleErrorAttribute
    {
        private readonly string ViewDataKey = "ViewData";
        private readonly string RotaOrigemKey = "RotaOrigem";

        public override void OnException(ExceptionContext filterContext)
        {
            var excecao = filterContext.Exception;

            if (excecao is ValidationException)
            {
                TratarErrosNegocio(excecao as ValidationException, filterContext);

                TratarApresentacao(filterContext);

                Log.Get().LogError(excecao);
            }
            else if (excecao is NegocioException)
            {
                TratarErrosNegocio(excecao as NegocioException, filterContext);

                TratarApresentacao(filterContext);

                Log.Get().LogError(excecao);
            }
            else if (excecao is DadosException)
            {
                TratarErrosNegocio(excecao as DadosException, filterContext);

                TratarApresentacao(filterContext);

                Log.Get().LogError(excecao);
            }
            else
            {
                filterContext.ExceptionHandled = false;
            }
        }

        #region Tratamento Apresentacao

        private void TratarApresentacao(ExceptionContext filterContext)
        {
            var rotaOrigem = filterContext.HttpContext.Request.UrlReferrer.GetRouteData();

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                var modelstate = filterContext.Controller.ViewData.ModelState;
                var resultado = modelstate.ObterErroModelState();
                var json = new JsonResult();
                json.Data = resultado;
                json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                filterContext.Result = json;
            }
            else
            {
                filterContext.HttpContext.Response.Clear();

                filterContext.Controller.TempData[ViewDataKey] = filterContext.Controller.ViewData;
                filterContext.Controller.TempData[RotaOrigemKey] = string.Format("{0}/{1}", rotaOrigem["controller"],
                    rotaOrigem["action"]);

                filterContext.Result = new RedirectToRouteResult(rotaOrigem);
            }
        }

        #endregion

        # region Tratamento Negocio

        private void TratarErrosNegocio(ValidationException excecao, ExceptionContext contexto)
        {
            foreach (var erro in excecao.Errors)
            {
                AddErro(erro.PropertyName, erro.ErrorMessage, contexto);
            }

            contexto.ExceptionHandled = true;
        }

        private void TratarErrosNegocio(NegocioException excecao, ExceptionContext contexto)
        {
            AddErro(excecao.Message, contexto);

            contexto.ExceptionHandled = true;
        }

        private void TratarErrosNegocio(DadosException excecao, ExceptionContext contexto)
        {
            AddErro(excecao.Message, contexto);

            contexto.ExceptionHandled = true;
        }

        private void AddErro(string mensagem, ExceptionContext contexto)
        {
            contexto.Controller.ViewData.ModelState.AddModelError(string.Empty, mensagem);
        }

        private void AddErro(string propriedade, string mensagem, ExceptionContext contexto)
        {
            contexto.Controller.ViewData.ModelState.AddModelError(propriedade, mensagem);
        }

        #endregion
    }
}