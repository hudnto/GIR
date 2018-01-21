using System;
using GIR.Core.Negocio.Enum;

namespace GIR.Core.Negocio.DTO
{
    public class InformeRendimentoDTO
    {
        public int Codigo { get; set; }
        public int Ano { get; set; }
        public DateTime Data { get; set; }
        public Int16 IdSituacao {
            get
            {
                return IdSituacao;
            }

            set
            {
                TipoSituacao = ConverterTipoSituacao(value);
            } 
        }
        public TipoSituacao TipoSituacao { get; set; }
        public String Descricao { get; set; }
        public ContribuinteDTO Contribuinte { get; set; }
        
        public static TipoSituacao ConverterTipoSituacao(Int16 situacao)
        {
            switch (situacao)
            {
                case 0:
                    return TipoSituacao.Processado;
                case 1:
                    return TipoSituacao.EnviadoWeb;
                default:
                    return TipoSituacao.Erro;
            }
        }
    }
}
