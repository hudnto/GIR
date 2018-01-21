using GASC.Seguranca;
using GIR.Core.Dados.Entidade;
using GIR.Core.Negocio.Consultas.QueryHandler;
using GIR.Core.Negocio.DTO;
using GIR.Core.Negocio.Enum;
using GIR.Core.Negocio.Mensagem;
using GIR.Core.Negocio.Servico;
using GIR.Intranet.Infraestructure.Extensions;
using GIR.Intranet.Infraestructure.Utilities;
using GIR.Intranet.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System;

namespace GIR.Intranet.Controllers
{
    public class ComprovanteRendimentoController : GenericController
    {
        private const string ComprovantesRendimento = "ComprovantesRendimento";
        private readonly GIRServico _girServico;
        private readonly RepositorioTempData _repositorio;

        public ComprovanteRendimentoController()
        {
            _girServico = new GIRServico();
            _repositorio = new RepositorioTempData(TempData);
        }

        [HttpGet]
        public ActionResult Index(int id)
        {
            var consulta = new ConsultaProcessamentoPeloIdQueryHandler();
            var processamentos = consulta.Execute(id);
            var processamentoDto = processamentos.ToList().First();

            _repositorio.Adicionar(ComprovantesRendimento, processamentoDto.Comprovantes);
            var model = VisualizaComprovanteVM.Converter(processamentoDto);
            return View(model);

        }

        [HttpPost]
        public JsonResult Visualizar(ConsultaComprovanteRendimentoVM model)
        {
            if (ModelState.IsValid)
            {
                var filtro = ConsultaComprovanteRendimentoVM.Converter(model);
                var consulta = new ConsultaComprovanteRendimentoDirfQueryHandler();
                var processamentos = consulta.Execute(filtro);
                _repositorio.Adicionar(ComprovantesRendimento, processamentos);

                return Json(new { sucesso = true });
            }
            else
            {
                var resultado = ModelState.ObterErroModelState();
                return Json(resultado);
            }
        }

        public JsonResult BuscarJson(int limit, int offset)
        {
            var query = _repositorio.Buscar(ComprovantesRendimento) as List<ComprovanteDTO>;
            if (query == null)
                return null;
            var total = query.Count();
            var lista = query.Skip(offset).Take(limit).ToList();

            var resultadoJson = lista.BootTableConsultaComprovantesRendimento(total);

            return Json(resultadoJson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Analisar(int id)
        {
            var consulta = new ConsultaProcessamentoPeloIdQueryHandler();
            var processamentos = consulta.Execute(id);
            var processamentoDto = processamentos.ToList().First();

            if (processamentoDto.TipoContribuinte == TipoContribuinte.FuncionarioCassi)
            {
                // TODO: Consultar Servico Colaborador e preencher retorno

                foreach (var comprovanteDto in processamentoDto.Comprovantes)
                {
                    const short retornoUf = 0;

                    if (comprovanteDto.Uf == 0)
                        comprovanteDto.Uf = retornoUf;

                    if (string.IsNullOrWhiteSpace(comprovanteDto.UnidadeOrganizacional))
                        comprovanteDto.UnidadeOrganizacional = "UnidadeOrganizacionalRetornoServico";

                    if (string.IsNullOrWhiteSpace(comprovanteDto.Email))
                        comprovanteDto.Email = "emailRetornoServico";
                }
            }
            else
            {
                // TODO: Consultar Servico Fornecedor e preencher retorno

                foreach (var comprovanteDto in processamentoDto.Comprovantes)
                {
                    const short retornoUf = 0;

                    if (comprovanteDto.Uf == 0)
                        comprovanteDto.Uf = retornoUf;

                    if (string.IsNullOrWhiteSpace(comprovanteDto.Email))
                        comprovanteDto.Email = "emailRetornoServico";
                }
            }

            _repositorio.Adicionar(ComprovantesRendimento, processamentoDto.Comprovantes);
            var model = VisualizaComprovanteVM.Converter(processamentoDto);

            return View("Index", model);
        }

        [HttpGet]
        public ActionResult AprovarInforme()
        {
            VisualizaComprovanteVM model = new VisualizaComprovanteVM();

            return View("Index", model);
        }

        [HttpPost]
        public ActionResult ModalAprovarInforme(string descricao, int analistaId)
        {
            var aprovarInformeDto = new Log()
            {
                DataRegistroAcao = DateTime.Today,
                DescricaoUsuario = descricao,
                Id = 0,
                LoginUsuario = ""
            };

            //_girServico.Salvar(aprovarInformeDto);

            return Json(new
            {
                sucesso = true,
                modal = new
                {
                    id = "confirmacao",
                    mensagem = Mensagem.MA005,
                    titulo = "Alerta de Confirmação",
                    txtConfirmar = "OK",
                }
            });
        }
    }
}

