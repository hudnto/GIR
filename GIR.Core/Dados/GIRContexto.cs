using GIR.Core.Dados.Entidade;
using System.Data.Entity;

namespace GIR.Core.Dados
{
    public class GIRContexto : DbContext
    {
        public GIRContexto()
            : base("name=GIRConexao")
        {
        }

        public virtual DbSet<Arquivo> Arquivos { get; set; }
        public virtual DbSet<Contribuinte> Contribuintes { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<Ocorrencia> Ocorrencias { get; set; }
        public virtual DbSet<Processamento> Processamentos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Arquivo>()
                .Property(e => e.CaminhoDiretorio)
                .IsUnicode(false);

            modelBuilder.Entity<Arquivo>()
                .Property(e => e.ExtensaoArquivo)
                .IsUnicode(false);

            modelBuilder.Entity<Arquivo>()
                .Property(e => e.NomeArquivo)
                .IsUnicode(false);

            modelBuilder.Entity<Contribuinte>()
                .Property(e => e.CPFCNPJ)
                .IsUnicode(false);

            modelBuilder.Entity<Contribuinte>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Contribuinte>()
                .Property(e => e.UnidadeOrganizacionalDescr)
                .IsUnicode(false);

            modelBuilder.Entity<Contribuinte>()
                .HasMany(e => e.Processamentos)
                .WithMany(e => e.Contribuintes)
                .Map(m => m.ToTable("TB_PRCSO_INFRS_RNDMO_CNTRE").MapLeftKey("ID_CNTRE").MapRightKey("ID_PRCSO_INFRE_RNDMO"));

            modelBuilder.Entity<Log>()
                .Property(e => e.LoginUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<Log>()
                .Property(e => e.DescricaoUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<Ocorrencia>()
                .Property(e => e.LoginUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<Ocorrencia>()
                .Property(e => e.DescricaoOcorrencia)
                .IsUnicode(false);

            modelBuilder.Entity<Processamento>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<Processamento>()
                .HasMany(e => e.Arquivos)
                .WithRequired(e => e.Processamento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Processamento>()
                .HasMany(e => e.Ocorrencias)
                .WithRequired(e => e.Processamento)
                .WillCascadeOnDelete(false);
        }
    }
}