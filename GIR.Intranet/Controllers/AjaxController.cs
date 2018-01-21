using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GIR.Core.Negocio.DTO;
using GIR.Core.Negocio.Servico;
using GIR.Integracao.Model;

namespace GIR.Intranet.Controllers
{
    public class AjaxController : Controller
    {
        private readonly UnidadeOrganizacionalServico _servico;

        public AjaxController()
        {
            _servico = new UnidadeOrganizacionalServico();
        }

        public JsonResult BuscarUnidadeOrganizacionalPorCodigo(int codigo)
        {
            var resultado = _servico.ObterUnidadeOrganizacional(codigo);
            var lista = new List<UnidadeOrganizacional> { resultado }; 
            var selectListUnidades = lista.Select(u => new SelectListItem()
            {
                Text = u.Nome.ToString(), Value = u.Codigo.ToString()
            });
            return Json(selectListUnidades, JsonRequestBehavior.AllowGet);
        }
    }
}