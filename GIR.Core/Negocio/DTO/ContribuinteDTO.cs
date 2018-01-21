using System;
using System.Collections.Generic;
using System.Linq;
using Cassi.Utilitarios.Util;
using GIR.Core.Dados;
using GIR.Core.Dados.Entidade;
using GIR.Core.Infraestrutura.Extensoes;
using GIR.Core.Negocio.Enum;

namespace GIR.Core.Negocio.DTO
{
    public class ContribuinteDTO
    {
        public String Nome { get; set; }
        public String CpfCnpj { get; set; }
        private Int16? _idTipo;
        public Int16? IdTipo
        {
            get
            {
                return _idTipo;
            }
            set
            {
                _idTipo = value;
                TipoContribuinte = ConverterTipoFuncionario(value);
            }
        }

        public TipoContribuinte TipoContribuinte { get; set; }

        public TipoSituacao TipoSituacao { get; set; }

        public StatusContribuinte Status { get; set; }

        public ArquivoDTO Arquivo { get; set; }

        public ContribuinteDTO()
        {
            Arquivo = new ArquivoDTO();
        }

        public static TipoContribuinte ConverterTipoFuncionario(Int16? tipo)
        {
            switch (tipo)
            {
                case 0:
                    return TipoContribuinte.FuncionarioCassi;
                case 1:
                    return TipoContribuinte.PrestadorFornecedor;
                default:
                    return TipoContribuinte.Nenhum;
            }
        }
    }
}