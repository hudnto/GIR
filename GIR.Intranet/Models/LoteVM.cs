using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GIR.Core.Negocio.DTO;
using GIR.Core.Negocio.Enum;
using GIR.Core.Negocio.Mensagem;

namespace GIR.Intranet.Models
{
    public class LoteVM
    {
        [Display(Name = "Código")]
        public int Codigo { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MA001")]
        public String Descricao { get; set; }

        [Display(Name = "Ano Exercício")]
        [Required(ErrorMessageResourceType = typeof(Mensagem), ErrorMessageResourceName = "MA001")]
        public int AnoExercicio { get; set; }

        [Display(Name = "Ano Calendário")]
        public int? AnoCalendario { get; set; }

        [Display(Name = "Individual")]
        public bool Individual { get; set; }

        [Display(Name = "Ocorrências")]
        public IEnumerable<String> Ocorrencias { get; set; }

        public string DataRegistro { get; set; }

        public String LoginUsuario { get; set; }

        public TipoContribuinte TipoContribuinte { get; set; }

        public TipoSituacao TipoSituacao { get; set; }

        public OperacaoGIR Operacao { get; set; }

        public IEnumerable<ArquivoVM> ArquivosImportados { get; set; }

        public int TotalArquivosGerados { get; set; }

        public LoteVM()
        {
            ArquivosImportados = new List<ArquivoVM>();
            Ocorrencias = new List<String>();
        }

        public static LoteDTO Converter(LoteVM origem, string loginUsuario)
        {
            var destino = new LoteDTO()
            {
                InicioProcessamento = DateTime.Now,
                AnoCalendario = origem.AnoCalendario ?? 0,
                AnoExercicio = origem.AnoExercicio,
                ArquivosImportados = ArquivoVM.Converter(origem.ArquivosImportados),
                Codigo = origem.Codigo,
                Descricao = origem.Descricao,
                LoginUsuario = loginUsuario,
                Individual = origem.Individual,
                Operacao = origem.Operacao,
                TipoContribuinte = origem.TipoContribuinte,
                TipoSituacao = origem.TipoSituacao
            };

            return destino;
        }

        public static LoteVM Converter(ProcessamentoDTO origem)
        {
            var model = new LoteVM()
            {
                Codigo = origem.Id,
                AnoExercicio = origem.AnoExercicio,
                AnoCalendario = origem.AnoCalendario,
                Descricao = origem.Descricao,
                TipoSituacao = origem.TipoSituacao,
                Individual = origem.Individual,
                TipoContribuinte = origem.TipoContribuinte ?? TipoContribuinte.Nenhum,
                TotalArquivosGerados = origem.TotalArquivosGerados,
                DataRegistro = origem.DataRegistro.ToString("g"),
                ArquivosImportados = origem.ArquivosImportados.Select(a => new ArquivoVM()
                {
                    NomeArquivo = a.NomeArquivo,
                    ExtensaoArquivo = a.ExtensaoArquivo
                }),
                Ocorrencias = origem.Ocorrencias,
                LoginUsuario = origem.LoginUsuario,
            };

            return model;
        }
    }
}