using System;

namespace GIR.Core.Negocio.DTO
{
    public class ComprovanteDTO
    {
        public int CodigoProcessamento { get; set; }
        public DateTime DataUltimaAtualizacao { get; set; }
        public int? UnidadeOrganizacionalId { get; set; }
        public string UnidadeOrganizacional { get; set; }
        public string CpfCnpj { get; set; }
        public string Email { get; set; }
        public short Uf { get; set; }
        public string NomeArquivo { get; set; }        
    }
}
