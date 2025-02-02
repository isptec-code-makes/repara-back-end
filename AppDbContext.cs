using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Repara.DAL.Migrations;

namespace Repara.DAL;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AgregadoFamiliare> AgregadoFamiliares { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Audit> Audits { get; set; }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Clube> Clubes { get; set; }

    public virtual DbSet<ClubeBanco> ClubeBancos { get; set; }

    public virtual DbSet<Cobranca> Cobrancas { get; set; }

    public virtual DbSet<Contacto> Contactos { get; set; }

    public virtual DbSet<ContactoGrupo> ContactoGrupos { get; set; }

    public virtual DbSet<ContactoGrupoRelacao> ContactoGrupoRelacaos { get; set; }

    public virtual DbSet<Inscricao> Inscricaos { get; set; }

    public virtual DbSet<InscricaoValidacao> InscricaoValidacaos { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Pagamento> Pagamentos { get; set; }

    public virtual DbSet<PagamentoValidacao> PagamentoValidacaos { get; set; }

    public virtual DbSet<Pessoa> Pessoas { get; set; }

    public virtual DbSet<PessoaPreferencia> PessoaPreferencias { get; set; }

    public virtual DbSet<Questionario> Questionarios { get; set; }

    public virtual DbSet<QuestionarioRespostum> QuestionarioResposta { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=PRINTF\\SQLEXPRESS;Database=SOGES;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AgregadoFamiliare>(entity =>
        {
            entity.HasIndex(e => e.PessoaId, "IX_AgregadoFamiliares_PessoaId");

            entity.Property(e => e.Apelido).HasMaxLength(100);
            entity.Property(e => e.Morada).HasMaxLength(200);
            entity.Property(e => e.Nome).HasMaxLength(100);

            entity.HasOne(d => d.Pessoa).WithMany(p => p.AgregadoFamiliares).HasForeignKey(d => d.PessoaId);
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Audit>(entity =>
        {
            entity.ToTable("Audit");

            entity.HasIndex(e => e.UserId, "IX_Audit_UserId");

            entity.Property(e => e.AffectedColumns).HasMaxLength(1000);
            entity.Property(e => e.NewValues).HasMaxLength(4000);
            entity.Property(e => e.OldValues).HasMaxLength(4000);
            entity.Property(e => e.PrimaryKey).HasMaxLength(128);
            entity.Property(e => e.TableName).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.Audits).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.Property(e => e.Beneficios).HasMaxLength(500);
            entity.Property(e => e.Designacao).HasMaxLength(150);
            entity.Property(e => e.Joia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Quota).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Clube>(entity =>
        {
            entity.ToTable("Clube");

            entity.Property(e => e.Entidade).HasMaxLength(6);
            entity.Property(e => e.Logo).HasMaxLength(150);
            entity.Property(e => e.Nif)
                .HasMaxLength(14)
                .HasColumnName("NIF");
            entity.Property(e => e.Nome).HasMaxLength(100);
            entity.Property(e => e.Sede).HasMaxLength(150);
        });

        modelBuilder.Entity<ClubeBanco>(entity =>
        {
            entity.ToTable("ClubeBanco");

            entity.HasIndex(e => e.ClubeId, "IX_ClubeBanco_ClubeId");

            entity.Property(e => e.Iban).HasMaxLength(25);
            entity.Property(e => e.NomeBanco).HasMaxLength(50);
            entity.Property(e => e.NomeConta).HasMaxLength(150);

            entity.HasOne(d => d.Clube).WithMany(p => p.ClubeBancos).HasForeignKey(d => d.ClubeId);
        });

        modelBuilder.Entity<Cobranca>(entity =>
        {
            entity.ToTable("Cobranca");

            entity.HasIndex(e => e.InscricaoId, "IX_Cobranca_InscricaoId");

            entity.HasIndex(e => e.PagamentoId, "IX_Cobranca_PagamentoId");

            entity.Property(e => e.Comentario).HasMaxLength(255);
            entity.Property(e => e.Desconto).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Montante).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Inscricao).WithMany(p => p.Cobrancas).HasForeignKey(d => d.InscricaoId);

            entity.HasOne(d => d.Pagamento).WithMany(p => p.Cobrancas).HasForeignKey(d => d.PagamentoId);
        });

        modelBuilder.Entity<Contacto>(entity =>
        {
            entity.ToTable("Contacto");

            entity.HasIndex(e => e.PessoaId, "IX_Contacto_PessoaId");

            entity.Property(e => e.Valor).HasMaxLength(255);

            entity.HasOne(d => d.Pessoa).WithMany(p => p.Contactos).HasForeignKey(d => d.PessoaId);
        });

        modelBuilder.Entity<ContactoGrupo>(entity =>
        {
            entity.ToTable("ContactoGrupo");

            entity.Property(e => e.Nome).HasMaxLength(150);
        });

        modelBuilder.Entity<ContactoGrupoRelacao>(entity =>
        {
            entity.ToTable("ContactoGrupoRelacao");

            entity.HasIndex(e => e.ContactoGrupoId, "IX_ContactoGrupoRelacao_ContactoGrupoId");

            entity.HasIndex(e => e.ContactoId, "IX_ContactoGrupoRelacao_ContactoId");

            entity.HasOne(d => d.ContactoGrupo).WithMany(p => p.ContactoGrupoRelacaos).HasForeignKey(d => d.ContactoGrupoId);

            entity.HasOne(d => d.Contacto).WithMany(p => p.ContactoGrupoRelacaos).HasForeignKey(d => d.ContactoId);
        });

        modelBuilder.Entity<Inscricao>(entity =>
        {
            entity.ToTable("Inscricao");

            entity.HasIndex(e => e.CategoriaId, "IX_Inscricao_CategoriaId");

            entity.HasIndex(e => e.PessoaId, "IX_Inscricao_PessoaId");

            entity.Property(e => e.Comentario).HasMaxLength(250);

            entity.HasOne(d => d.Categoria).WithMany(p => p.Inscricaos).HasForeignKey(d => d.CategoriaId);

            entity.HasOne(d => d.Pessoa).WithMany(p => p.Inscricaos).HasForeignKey(d => d.PessoaId);
        });

        modelBuilder.Entity<InscricaoValidacao>(entity =>
        {
            entity.ToTable("InscricaoValidacao");

            entity.HasIndex(e => e.InscricaoId, "IX_InscricaoValidacao_InscricaoId");

            entity.HasIndex(e => e.UserId, "IX_InscricaoValidacao_UserId");

            entity.Property(e => e.Comentario).HasMaxLength(250);

            entity.HasOne(d => d.Inscricao).WithMany(p => p.InscricaoValidacaos).HasForeignKey(d => d.InscricaoId);

            entity.HasOne(d => d.User).WithMany(p => p.InscricaoValidacaos).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("Notification");

            entity.HasIndex(e => e.ContactoId, "IX_Notification_ContactoId");

            entity.HasIndex(e => e.UserId, "IX_Notification_UserId");

            entity.Property(e => e.Assunto).HasMaxLength(255);

            entity.HasOne(d => d.Contacto).WithMany(p => p.Notifications).HasForeignKey(d => d.ContactoId);

            entity.HasOne(d => d.User).WithMany(p => p.Notifications).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Pagamento>(entity =>
        {
            entity.ToTable("Pagamento");

            entity.Property(e => e.Anexo).HasMaxLength(36);
            entity.Property(e => e.Comentario).HasMaxLength(255);
            entity.Property(e => e.Referencia).HasMaxLength(50);
            entity.Property(e => e.ValorPago).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ValorTotal).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<PagamentoValidacao>(entity =>
        {
            entity.ToTable("PagamentoValidacao");

            entity.HasIndex(e => e.PagamentoId, "IX_PagamentoValidacao_PagamentoId").IsUnique();

            entity.HasIndex(e => e.UserIdPagamentoVaidacao, "IX_PagamentoValidacao_UserIdPagamentoVaidacao");

            entity.HasOne(d => d.Pagamento).WithOne(p => p.PagamentoValidacao).HasForeignKey<PagamentoValidacao>(d => d.PagamentoId);

            entity.HasOne(d => d.UserIdPagamentoVaidacaoNavigation).WithMany(p => p.PagamentoValidacaos).HasForeignKey(d => d.UserIdPagamentoVaidacao);
        });

        modelBuilder.Entity<Pessoa>(entity =>
        {
            entity.ToTable("Pessoa");

            entity.HasIndex(e => e.UserId, "IX_Pessoa_UserId");

            entity.Property(e => e.Apelido).HasMaxLength(100);
            entity.Property(e => e.Bairro).HasMaxLength(150);
            entity.Property(e => e.Codigo).HasMaxLength(50);
            entity.Property(e => e.DocumentoIdentificacaoFile).HasMaxLength(150);
            entity.Property(e => e.Fotografia).HasMaxLength(150);
            entity.Property(e => e.Morada).HasMaxLength(150);
            entity.Property(e => e.Municipio).HasMaxLength(50);
            entity.Property(e => e.Nacionalidade).HasMaxLength(50);
            entity.Property(e => e.Nome).HasMaxLength(100);
            entity.Property(e => e.NumeroDocumentoIdentificacao).HasMaxLength(30);
            entity.Property(e => e.Pais).HasMaxLength(50);
            entity.Property(e => e.Provincia).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.Pessoas).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<PessoaPreferencia>(entity =>
        {
            entity.HasIndex(e => e.PessoaId, "IX_PessoaPreferencias_PessoaId").IsUnique();

            entity.HasOne(d => d.Pessoa).WithOne(p => p.PessoaPreferencia).HasForeignKey<PessoaPreferencia>(d => d.PessoaId);
        });

        modelBuilder.Entity<Questionario>(entity =>
        {
            entity.ToTable("Questionario");

            entity.Property(e => e.Placeholder).HasMaxLength(100);
            entity.Property(e => e.PossiveisValores).HasMaxLength(250);
            entity.Property(e => e.Questao).HasMaxLength(150);
        });

        modelBuilder.Entity<QuestionarioRespostum>(entity =>
        {
            entity.HasIndex(e => e.PessoaId, "IX_QuestionarioResposta_PessoaId");

            entity.HasIndex(e => e.QuestionarioId, "IX_QuestionarioResposta_QuestionarioId");

            entity.HasOne(d => d.Pessoa).WithMany(p => p.QuestionarioResposta).HasForeignKey(d => d.PessoaId);

            entity.HasOne(d => d.Questionario).WithMany(p => p.QuestionarioResposta).HasForeignKey(d => d.QuestionarioId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
