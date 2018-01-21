using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using GIR.Core.Negocio.DTO;

namespace GIR.Intranet.Models
{
    public class ComprovanteVM
    {
        public DateTime DataUltimaAtualizacao { get; set; }

        [DisplayName(@"Unid:")]
        public int? UnidadeOrganizacionalId { get; set; }

        [DisplayName(@"Descrição:")]
        public string UnidadeOrganizacional { get; set; }

        [DisplayName(@"CPF/CNPJ:")]
        public string CpfCnpj { get; set; }

        public string Email { get; set; }

        [DisplayName(@"UF:")]
        public string Uf { get; set; }

        public string NomeArquivo { get; set; }

        public static IEnumerable<ComprovanteVM> Converter(IEnumerable<ComprovanteDTO> dtos)
        {
            return dtos.Select(dto => new ComprovanteVM
            {
                NomeArquivo = dto.NomeArquivo, 
                CpfCnpj = dto.CpfCnpj, 
                DataUltimaAtualizacao = dto.DataUltimaAtualizacao,
                Uf = dto.Uf.ToString(),
                UnidadeOrganizacional = dto.UnidadeOrganizacional,
                UnidadeOrganizacionalId = dto.UnidadeOrganizacionalId,
                Email = dto.Email
            }).ToList();
        }
    }
}