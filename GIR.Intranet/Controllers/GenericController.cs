using GIR.Core.Negocio.Erro;
using System;

namespace GIR.Intranet.Controllers
{
    public abstract class GenericController : GASC.Seguranca.Generic.GenericController
    {
        internal const string KeyMensagemAlerta = "key.mensagem.alerta";
        internal const string KeyMensagemErro = "key.mensagem.erro";

        /// <summary>
        /// Adiciona uma mensagem na ViewBag.Mensagem para ser
        /// apresentada na página "_Layout.cshtml";
        /// </summary>
        /// <param name="mensagem">Mensagem a ser apresentada.</param>
        internal void ExibirAlerta(string mensagem, bool isSucesso = false)
        {
            if (!string.IsNullOrEmpty(mensagem))
            {
                ViewBag.Mensagem = mensagem;
                ViewBag.IsMensagemSucesso = isSucesso;
            }
        }

        /// <summary>
        /// Adiciona uma mensagem no objeto ModelState.AddModelError 
        /// para ser apresentada no componente "@Html.ValidationSummary()"
        /// colocado na página "_Layout.cshtml";
        /// </summary>
        /// <param name="mensagem">Mensagem de erro a ser apresentada.</param>
        internal void ExibirErro(string mensagem)
        {
            if (!string.IsNullOrEmpty(mensagem))
            {
                ModelState.AddModelError(string.Empty, mensagem);
            }
        }

        /// <summary>
        /// Recupera um Item colocado no componente TempData[key]
        /// pelo key informado por parâmetro.
        /// Esse componente mantem o item na request/response;
        /// </summary>
        /// <param name="keyItem">Key do item.</param>
        /// <returns></returns>
        internal string GetItemRedirect(string keyItem)
        {
            if (TempData.ContainsKey(keyItem))
            {
                return TempData[keyItem] as string;
            }
            return string.Empty;
        }

        /// <summary>
        /// Adiciona um Item (valorItem) no componente TempData[key]
        /// identificado pelo Key (keyItem), utilizando no RedirectToAction
        /// pode ser recuperado na Action seguinte chamada.
        /// </summary>
        /// <param name="keyItem">Key do item armazenado.</param>
        /// <param name="valorItem">Valor do item armazenado.</param>
        internal void AddItemRedirect(string keyItem, string valorItem)
        {
            TempData.Add(keyItem, valorItem);
        }

        /// <summary>
        /// Tratar as mensagens de erro de negócio.
        /// </summary>
        /// <param name="erro">Exception de erro para tratamento.</param>
        /// <param name="localErro"></param>
        internal void TratarErroNegocio(Exception erro, string localErro = null)
        {
            AppLogger.Log.Get().LogError(erro, localErro);
            if (erro.GetType() == typeof(NegocioException))
            {
                ExibirErro((erro as NegocioException).Message);
            }
            else
            {
                ExibirErro((localErro != null ? localErro + ": " : "") + "Erro no processamento da solicitação.");
            }
        }     
    }
}
