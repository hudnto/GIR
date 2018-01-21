using GIR.Core.Dados.Interface;

namespace GIR.Core.Dados.Entidade
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TB_PRCSO_INFRE_RNDMO")]
    public class Processamento : IEntidade
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Processamento()
        {
            Arquivos = new HashSet<Arquivo>();
            Ocorrencias = new HashSet<Ocorrencia>();
            Contribuintes = new HashSet<Contribuinte>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("ID_PRCSO_INFRE_RNDMO")]
        public int Id { get; set; }

        [Column("CD_TIPO_STCO_INFRE_RNDMO")]
        public Int16 SituacaoProcessamento { get; set; }

        [Column("AA_EXRCO_INFRE_RNDMO")]
        public int AnoExercicio { get; set; }

        [Column("AA_CLNDO_INFRE_RNDMO")]
        public int AnoCalendario { get; set; }

        [Required, StringLength(50), Column("TX_DSCRO_INFRE_RNDMO")]
        public string Descricao { get; set; }

        [Column("IN_INFRE_RNDMO_INDVL")]
        public bool Individual { get; set; }

        [Column("DH_RGSTO_INFRE_RNDMO")]
        public DateTime DataRegistro { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Arquivo> Arquivos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ocorrencia> Ocorrencias { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contribuinte> Contribuintes { get; set; }
    }
}