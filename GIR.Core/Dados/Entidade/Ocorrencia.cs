using GIR.Core.Dados.Interface;

namespace GIR.Core.Dados.Entidade
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TB_OCRNA_PRCSO_INFRE_RNDMO")]
    public class Ocorrencia : IEntidade
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("ID_OCRNA_PRCSO_INFRE_RNDMO")]
        public int Id { get; set; }

        [Column("ID_PRCSO_INFRE_RNDMO")]
        public int ProcessamentoId { get; set; }

        [Column("CD_TIPO_OCRNA_INFRE_RNDMO")]
        public short SituacaoId { get; set; }

        [Required, StringLength(50), Column("CD_LGN_USRO")]
        public string LoginUsuario { get; set; }

        [Required, StringLength(1000), Column("TX_DSCRO_OCRNA")]
        public string DescricaoOcorrencia { get; set; }

        public virtual Processamento Processamento { get; set; }
    }
}
