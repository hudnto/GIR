using GIR.Core.Dados.Interface;

namespace GIR.Core.Dados.Entidade
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TB_ARQVO_INFRS_RNDMO")]
    public class Arquivo: IEntidade
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("ID_ARQVO_INFRS_RNDMO")]
        public int Id { get; set; }

        [Column("ID_PRCSO_INFRE_RNDMO")]
        public int ProcessamentoId { get; set; }

        [Column("CD_TIPO_STCO_INFRE_RNDMO")]
        public short SituacaoId { get; set; }

        [Required, StringLength(200), Column("TX_CMNHO_DRTRO")]
        public string CaminhoDiretorio { get; set; }

        [Required, StringLength(4), Column("TX_EXTNO_ARQVO")]
        public string ExtensaoArquivo { get; set; }

        [Column("DH_ULTMA_ATLZO_ARQVO")]
        public DateTime DataUltimaAtualizacao { get; set; }

        [Required, StringLength(50), Column("TX_NOME_ARQVO")]
        public string NomeArquivo { get; set; }

        public virtual Processamento Processamento { get; set; }
    }
}
