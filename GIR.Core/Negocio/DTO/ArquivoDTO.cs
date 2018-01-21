using System;
using System.Collections.Generic;
using System.Linq;
using GIR.Core.Dados;
using GIR.Core.Dados.Entidade;

namespace GIR.Core.Negocio.DTO
{
    public class ArquivoDTO
    {
        public int ArquivoId { get; set; }
        public short SituacaoId { get; set; }
        public string CaminhoArquivo { get; set; }
        public string CaminhoArquivoBanco { get; set; }
        public string ExtensaoArquivo { get; set; }
        public string NomeArquivo { get; set; }
        public bool FoiEncontrado { get; set; }
        public bool ArquivoFalhouAoImportar { get; set; }
        public byte[] Bytes { get; set; }
        public DateTime? DataUltimaAtualizacao { get; set; }

        public static ArquivoDTO Converter(string nomeArquivo)
        {
            var dto = new ArquivoDTO()
            {
                NomeArquivo = nomeArquivo
            };

            return dto;
        }
    }
}
