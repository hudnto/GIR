using GIR.Core.Dados.Interface;

namespace GIR.Core.Dados.Entidade
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TB_CNTRE")]
    public class Contribuinte : IEntidade
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Contribuinte()
        {
            Processamentos = new HashSet<Processamento>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("ID_CNTRE")]
        public int Id { get; set; }

        [Column("CD_ESTDO")]
        public short? EstadoId { get; set; }

        [Column("CD_TIPO_CNTRE")]
        public short TipoContribuinteId { get; set; }

        [Required, StringLength(14), Column("CD_CPF_CNPJ")]
        public string CPFCNPJ { get; set; }

        [StringLength(100), Column("TX_EMAIL")]
        public string Email { get; set; }

        [Column("CD_UNDDE_ORGNL")]
        public int? UnidadeOrganizacional { get; set; }

        [Column("TX_DSCRO_UNDDE_ORGNL"), StringLength(100)]
        public string UnidadeOrganizacionalDescr { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Processamento> Processamentos { get; set; }
    }
}