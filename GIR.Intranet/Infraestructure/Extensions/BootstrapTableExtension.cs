using System.Collections.Generic;
using GIR.Core.Infraestrutura.Extensoes;
using GIR.Core.Negocio.DTO;

namespace GIR.Intranet.Infraestructure.Extensions
{
    public static class BootstrapTableExtension
    {
        public static object BootTableConsultaInformes(this List<InformeRendimentoDTO> lista, int total)
        {
            var rows = new List<object>();
            foreach (var item in lista)
            {
                var linha = new
                {
                    item.Codigo,
                    item.Ano,
                    item.Descricao,
                    Data = item.Data.ToString("d"),
                    Situacao = item.TipoSituacao.GetDescription(),
                    Tipo = item.Contribuinte.TipoContribuinte.GetDescription()
                };
                rows.Add(linha);
            }
            var resultado = new { total, rows };
            return resultado;
        }

        public static object BootTableConsultaInformesColaborador(this List<InformeColaboradorDTO> lista, int total)
        {
            var rows = new List<object>();
            foreach (var item in lista)
            {
                var linha = new
                {
                    item.AnoExercicio,
                    item.CPFCNPJ
                };
                rows.Add(linha);
            }
            var resultado = new { total, rows };
            return resultado;
        }

        public static object BootTableConsultaComprovantesRendimento(this List<ComprovanteDTO> lista, int total)
        {
            var rows = new List<object>();
            foreach (var item in lista)
            {
                var linha = new
                {
                    item.CodigoProcessamento,
                    item.NomeArquivo,
                    item.CpfCnpj,
                    DataUltimaAtualizacao = item.DataUltimaAtualizacao.ToString("d"),
                    item.Uf,
                    item.UnidadeOrganizacional,
                    item.Email
                };
                rows.Add(linha);
            }
            var resultado = new { total, rows };
            return resultado;
        }
    }
}