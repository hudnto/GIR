using System;
using System.Collections.Generic;
using GIR.Core.Negocio.DTO;

namespace GIR.Intranet.Models
{
    public class ArquivoVM
    {
        public string CaminhoArquivo { get; set; }
        public string ExtensaoArquivo { get; set; }
        public string NomeArquivo { get; set; }
        public bool ArquivoFalhouAoImportar { get; set; }

        public static IEnumerable<ArquivoDTO> Converter(IEnumerable<ArquivoVM> origem)
        {
            var destino = new List<ArquivoDTO>();

            foreach (var item in origem)
            {
                destino.Add(new ArquivoDTO()
                {
                    ArquivoFalhouAoImportar = item.ArquivoFalhouAoImportar,
                    CaminhoArquivo = item.CaminhoArquivo,
                    ExtensaoArquivo = item.ExtensaoArquivo,
                    NomeArquivo = item.NomeArquivo
                });
            }

            return destino;
        }
    }
}