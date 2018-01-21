using System.Web.Mvc;
using GASC.Seguranca;
using GIR.Intranet.Infraestructure.Filters;

namespace GIR.Intranet
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CassiAuthorizeAttribute());
            filters.Add(new RestaurarViewDataAposExcecaoAttribute());
            filters.Add(new GlobalErrorHandlerAttribute());
            //Caso haja desinstalação do GASC favor retirar a linha acima e retirar o comentário da linha abaixo
            //filters.Add(new HandleErrorAttribute());
        }
    }
}