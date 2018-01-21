using GASC.Seguranca;
using System.Web.Mvc;
using GIR.Core.Negocio.Enum;

namespace GIR.Intranet.Controllers
{
    [CassiAuthorize(Sistema = GascFuncionalidade.Sistema)]
    public class HomeController : GenericController
    {
        [HttpGet]
        public ActionResult Index()
        {
            if (User.IsInFuncionalidade(GascFuncionalidade.GR01))
                return RedirectToAction("ConsultarInformeRendimento", "Processamento");

            if (User.IsInFuncionalidade(GascFuncionalidade.GR08))
                return RedirectToAction("Index", "InformeColaborador");

            return View();
        }
        
        public ActionResult Logoff()
        {
            CassiAuthorizeUtil.Logoff();
            return RedirectToAction("Index");
        }
    }
}
