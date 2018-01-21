using GIR.Core.Negocio.Consultas.QueryHandler;
using GIR.Core.Negocio.DTO;
using GIR.Intranet.Infraestructure.Extensions;
using GIR.Intranet.Infraestructure.Utilities;
using GIR.Intranet.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using GASC.Seguranca;
using GIR.Core.Negocio.Enum;

namespace GIR.Intranet.Controllers
{
    //[CassiAuthorize(Funcionalidades = GascFuncionalidade.GR08)]
    public class InformeColaboradorController : GenericController
    {
        private const String InformesColaborador = "InformesColaborador";
        private RepositorioTempData _repositorio;        

        public InformeColaboradorController()
        {
            _repositorio = new RepositorioTempData(TempData);
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Index(ConsultaInformeColaboradorVM model)
        {
            if (ModelState.IsValid)
            {
                var filtro = ConsultaInformeColaboradorVM.Converter(model);
                var consulta = new ConsultaInformeColaboradorQueryHandler();
                var informes = consulta.Execute(filtro);
                _repositorio.Adicionar(InformesColaborador, informes);
                
                return Json(new { sucesso = true });
            }
            else
            {
                var resultado = ModelState.ObterErroModelState();
                return Json(resultado);
            }
        }

        public JsonResult BuscarJSON(int limit, int offset)
        {
            var query = _repositorio.Buscar(InformesColaborador) as IQueryable<InformeColaboradorDTO>;
            if (query == null)
                return null;
            var total = query.Count();
            var lista = query.Skip(offset).Take(limit).ToList();

            var resultadoJSON = lista.BootTableConsultaInformesColaborador(total);

            return Json(resultadoJSON, JsonRequestBehavior.AllowGet);
        }
    }
}
