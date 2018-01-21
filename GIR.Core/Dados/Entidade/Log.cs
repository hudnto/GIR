using GIR.Core.Dados.Interface;

namespace GIR.Core.Dados.Entidade
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TB_LOG_ACAO_USRO")]
    public class Log : IEntidade
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("ID_LOG_ACAO_USRO")]
        public int Id { get; set; }

        [Required, StringLength(50), Column("CD_LGN_USRO")]
        public string LoginUsuario { get; set; }

        [Required, StringLength(300), Column("TX_DSCRO_ACAO_USRO")]
        public string DescricaoUsuario { get; set; }

        [Column("DH_RGSTO_ACAO_USRO")]
        public DateTime DataRegistroAcao { get; set; }
    }
}
