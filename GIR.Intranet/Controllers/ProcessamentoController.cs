using GIR.Core.Negocio.Servico;
using GIR.Intranet.Models;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using GASC.Seguranca;
using GIR.Core.Negocio.Consultas.QueryHandler;
using GIR.Core.Negocio.DTO;
using GIR.Core.Negocio.Enum;
using GIR.Core.Negocio.Mensagem;
using GIR.Intranet.Infraestructure.Extensions;
using GIR.Intranet.Infraestructure.Utilities;

namespace GIR.Intranet.Controllers
{
    //[CassiAuthorize(Funcionalidades = GascFuncionalidade.GR01)]
    public class ProcessamentoController : GenericController
    {
        private readonly ArquivoServico _arquivoServico;
        private readonly RepositorioTempData _repositorio;
        private const String ArquivosUpload = "ArquivosUpload";
        private const String Informes = "Informes";

        public ProcessamentoController()
        {
            _arquivoServico = new ArquivoServico();
            _repositorio = new RepositorioTempData(TempData);
        }

        [HttpGet]
        public ActionResult ConsultarInformeRendimento()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ConsultarInformeRendimento(ConsultaInformeVM model)
        {
            if (ModelState.IsValid)
            {
                var filtro = ConsultaInformeVM.Converter(model);
                var consulta = new ConsultaInformeQueryHandler();
                var informes = consulta.Execute(filtro);

                _repositorio.Adicionar(Informes, informes);

                return Json(new { sucesso = true });
            }

            var resultado = ModelState.ObterErroModelState();
            return Json(resultado);
        }

        [HttpGet]
        public ActionResult Novo()
        {
            var arquivos = _repositorio.Buscar<List<ArquivoVM>>(ArquivosUpload);

            if (arquivos != null)
            {
                var path = string.Format(@"{0}\{1}", _arquivoServico.CaminhoDiretorio, ArquivosServicoBase.PastaTempUpload);
                _arquivoServico.RemoverDiretorio(path);
                _repositorio.Excluir(ArquivosUpload);
            }

            return View();
        }

        [HttpPost]
        public ActionResult Novo(LoteVM lote)
        {
            var arquivosArmazenados = _repositorio.Buscar<List<ArquivoVM>>(ArquivosUpload);

            ValidaSeHouveUpload(arquivosArmazenados, lote.Individual);

            if (arquivosArmazenados != null)
                lote.ArquivosImportados = arquivosArmazenados;

            if (ModelState.IsValid)
            {
                var dto = LoteVM.Converter(lote, User.LoginUsuario);

                _arquivoServico.GerarProcessamento(dto);
                _repositorio.Excluir(ArquivosUpload);

                ViewBag.IsMensagemSucesso = true;
                ViewBag.Mensagem = "Registro incluído com sucesso";
            }

            return View(lote);
        }

        [HttpGet]
        public ActionResult Rotina(int id)
        {
            var consulta = new ConsultaProcessamentoQueryHandler();
            var processamento = consulta.Execute(id).FirstOrDefault();

            if (processamento == null)
                return RedirectToAction("Novo");
            
            var model = LoteVM.Converter(processamento);

            return View(model);
        }

        public JsonResult BuscarInformeRendimentoJSON(int limit, int offset)
        {
            var query = _repositorio.Buscar<IQueryable<InformeRendimentoDTO>>(Informes);
            if (query == null)
                return null;
            var total = query.Count();
            var lista = query.Skip(offset).Take(limit).ToList();

            var resultadoJSON = lista.BootTableConsultaInformes(total);

            return Json(resultadoJSON, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult VerificaSeExisteProcessamentoJSON(int? anoExercicio, TipoContribuinte? tipo)
        {
            if (anoExercicio == null || tipo == null)
                return Json(new { sucesso = false, id = 0 }, JsonRequestBehavior.AllowGet);

            var ano = anoExercicio.Value;
            var tipoContribuinte = tipo.Value;

            var filtro = ProcessamentoDTO.Converter(ano, tipoContribuinte);
            var consulta = new ConsultaProcessamentoQueryHandler();
            var processamento = consulta.Execute(filtro).FirstOrDefault();

            if (processamento != null)
            {
                return Json(new { sucesso = true, id = processamento.Id }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { sucesso = false, id = 0 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadArquivosJSON(AnexoVM model)
        {
            if (ModelState.IsValid)
            {
                var contentType = model.TipoArquivo;
                var arquivos = _repositorio.Buscar<List<ArquivoVM>>(ArquivosUpload);

                if (arquivos != null)
                    ArquivoController.ValidaEnvioArquivosDuplicados(arquivos, model.Arquivos, _arquivoServico);
                else
                    arquivos = new List<ArquivoVM>();

                var arquivosArmazenados = ArquivoController.ArmazenarArquivosParaProcessamento(model.Arquivos, _arquivoServico);

                var arquivosUpload = arquivos.Concat(arquivosArmazenados).ToList();

                _repositorio.Adicionar(ArquivosUpload, arquivosUpload);

                return Json(new { sucesso = true, contentType });
            }

            return Json(ModelState.ObterErroModelState());
        }

        [HttpGet]
        public ActionResult ArquivosArmazenadosJSON(string tipo)
        {
            var arquivos = _repositorio.Buscar<List<ArquivoVM>>(ArquivosUpload);

            if (arquivos == null || tipo == null)
                return Json(new { arquivos = new List<ArquivoVM>() });

            var filtro = (tipo == "application/pdf" ? ".pdf" : ".txt");

            var arquivosEncontrados = arquivos.Where(a => a.ExtensaoArquivo == filtro).ToList();
            var arquivosFail = arquivos.Where(a => a.ArquivoFalhouAoImportar).ToList();

            var files = arquivosEncontrados.Concat(arquivosFail).ToList();

            return Json(new { arquivos = files.ParseParaJson() }, JsonRequestBehavior.AllowGet);
        }

        private void ValidaSeHouveUpload(IEnumerable<ArquivoVM> arquivosArmazenados, bool individual)
        {
            if (individual)
            {
                if (arquivosArmazenados == null)
                {
                    ModelState.AddModelError("PDF", String.Format(Mensagem.MA001, "Arquivos PDF"));
                    return;
                }

                if (arquivosArmazenados.Any(a => a.ExtensaoArquivo != ".pdf"))
                    ModelState.AddModelError("PDF", String.Format(Mensagem.MA001, "Arquivos PDF"));
            }
            else
            {
                if (arquivosArmazenados == null)
                {
                    ModelState.AddModelError("PDF", String.Format(Mensagem.MA001, "Arquivos PDF"));
                    ModelState.AddModelError("TXT", String.Format(Mensagem.MA001, "Arquivo gerador da DIRF (txt)"));
                    return;
                }

                if (!arquivosArmazenados.Any(a => a.ExtensaoArquivo == ".txt"))
                    ModelState.AddModelError("TXT", String.Format(Mensagem.MA001, "Arquivo gerador da DIRF (txt)"));

                if (!arquivosArmazenados.Any(a => a.ExtensaoArquivo == ".pdf"))
                    ModelState.AddModelError("PDF", String.Format(Mensagem.MA001, "Arquivos PDF"));
            }
        }
    }
}