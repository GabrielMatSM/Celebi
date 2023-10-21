using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Manager.Models;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Contapendente> Contapendentes { get; set; }

    public virtual DbSet<Funcionario> Funcionarios { get; set; }

    public virtual DbSet<Itempedido> Itempedidos { get; set; }

    public virtual DbSet<Notafiscal> Notafiscals { get; set; }

    public virtual DbSet<Pessoa> Pessoas { get; set; }

    public virtual DbSet<Produto> Produtos { get; set; }

    public virtual DbSet<Situacaonotum> Situacaonota { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=celebi.cauvkcrxijoi.us-east-1.rds.amazonaws.com;Database=postgres;Username=postgres;Port=5433;Password=102807561");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Clienteid).HasName("cliente_pkey");

            entity.ToTable("cliente");

            entity.Property(e => e.Clienteid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("clienteid");
            entity.Property(e => e.Ativo).HasColumnName("ativo");
            entity.Property(e => e.Dividaativa).HasColumnName("dividaativa");
            entity.Property(e => e.Limitecredito).HasColumnName("limitecredito");
            entity.Property(e => e.Pessoaid).HasColumnName("pessoaid");

            entity.HasOne(d => d.Pessoa).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.Pessoaid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cliente_fk");
        });

        modelBuilder.Entity<Contapendente>(entity =>
        {
            entity.HasKey(e => e.Contapendenteidint).HasName("contapendente_pkey");

            entity.ToTable("contapendente");

            entity.Property(e => e.Contapendenteidint)
                .UseIdentityAlwaysColumn()
                .HasColumnName("contapendenteidint");
            entity.Property(e => e.Clienteid).HasColumnName("clienteid");
            entity.Property(e => e.Dataprevistadepagamento)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dataprevistadepagamento");
            entity.Property(e => e.Notafiscalid).HasColumnName("notafiscalid");
            entity.Property(e => e.Valorpendente).HasColumnName("valorpendente");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Contapendentes)
                .HasForeignKey(d => d.Clienteid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("contapendente_fk2");

            entity.HasOne(d => d.Notafiscal).WithMany(p => p.Contapendentes)
                .HasForeignKey(d => d.Notafiscalid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("contapendente_fk");
        });

        modelBuilder.Entity<Funcionario>(entity =>
        {
            entity.HasKey(e => e.Funcionarioid).HasName("funcionario_pkey");

            entity.ToTable("funcionario");

            entity.Property(e => e.Funcionarioid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("funcionarioid");
            entity.Property(e => e.Adm).HasColumnName("adm");
            entity.Property(e => e.Login)
                .HasMaxLength(70)
                .HasColumnName("login");
            entity.Property(e => e.Pessoaid).HasColumnName("pessoaid");
            entity.Property(e => e.Senha)
                .HasMaxLength(100)
                .HasColumnName("senha");

            entity.HasOne(d => d.Pessoa).WithMany(p => p.Funcionarios)
                .HasForeignKey(d => d.Pessoaid)
                .HasConstraintName("funcionario_fk");
        });

        modelBuilder.Entity<Itempedido>(entity =>
        {
            entity.HasKey(e => e.Itempedidoid).HasName("itempedido_pkey");

            entity.ToTable("itempedido");

            entity.Property(e => e.Itempedidoid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("itempedidoid");
            entity.Property(e => e.Notafiscalid).HasColumnName("notafiscalid");
            entity.Property(e => e.Precototal).HasColumnName("precototal");
            entity.Property(e => e.Produtoid).HasColumnName("produtoid");
            entity.Property(e => e.Quantidade).HasColumnName("quantidade");

            entity.HasOne(d => d.Notafiscal).WithMany(p => p.Itempedidos)
                .HasForeignKey(d => d.Notafiscalid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("itempedido_fk2");

            entity.HasOne(d => d.Produto).WithMany(p => p.Itempedidos)
                .HasForeignKey(d => d.Produtoid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("itempedido_fk1");
        });

        modelBuilder.Entity<Notafiscal>(entity =>
        {
            entity.HasKey(e => e.Notafiscalid).HasName("notafiscal_pkey");

            entity.ToTable("notafiscal");

            entity.Property(e => e.Notafiscalid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("notafiscalid");
            entity.Property(e => e.Data)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("data");
            entity.Property(e => e.Funcionarioid).HasColumnName("funcionarioid");
            entity.Property(e => e.Situacaonotaid).HasColumnName("situacaonotaid");
            entity.Property(e => e.Valorpago).HasColumnName("valorpago");
            entity.Property(e => e.Valortotal).HasColumnName("valortotal");

            entity.HasOne(d => d.Funcionario).WithMany(p => p.Notafiscals)
                .HasForeignKey(d => d.Funcionarioid)
                .HasConstraintName("notafiscal_fk1");

            entity.HasOne(d => d.Situacaonota).WithMany(p => p.Notafiscals)
                .HasForeignKey(d => d.Situacaonotaid)
                .HasConstraintName("notafiscal_fk2");
        });

        modelBuilder.Entity<Pessoa>(entity =>
        {
            entity.HasKey(e => e.Pessoaid).HasName("pessoa_pkey");

            entity.ToTable("pessoa");

            entity.Property(e => e.Pessoaid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("pessoaid");
            entity.Property(e => e.Cep)
                .HasMaxLength(20)
                .HasColumnName("cep");
            entity.Property(e => e.Cidade)
                .HasMaxLength(40)
                .HasColumnName("cidade");
            entity.Property(e => e.Contato)
                .HasMaxLength(20)
                .HasColumnName("contato");
            entity.Property(e => e.Cpf)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("cpf");
            entity.Property(e => e.Endereco)
                .HasMaxLength(200)
                .HasColumnName("endereco");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasColumnName("estado");
            entity.Property(e => e.Idade).HasColumnName("idade");
            entity.Property(e => e.Nome)
                .HasMaxLength(70)
                .HasColumnName("nome");
            entity.Property(e => e.Rg)
                .HasMaxLength(15)
                .HasColumnName("rg");
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(e => e.Produtoid).HasName("produto_pkey");

            entity.ToTable("produto");

            entity.Property(e => e.Produtoid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("produtoid");
            entity.Property(e => e.Ativo)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("ativo");
            entity.Property(e => e.Descricao)
                .HasMaxLength(70)
                .HasColumnName("descricao");
            entity.Property(e => e.Fornecedor)
                .HasMaxLength(80)
                .HasColumnName("fornecedor");
            entity.Property(e => e.Preco).HasColumnName("preco");
            entity.Property(e => e.Quantidadeemestoque).HasColumnName("quantidadeemestoque");
        });

        modelBuilder.Entity<Situacaonotum>(entity =>
        {
            entity.HasKey(e => e.Situacaonotaid).HasName("situacaonota_pkey");

            entity.ToTable("situacaonota");

            entity.Property(e => e.Situacaonotaid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("situacaonotaid");
            entity.Property(e => e.Descricao)
                .HasMaxLength(50)
                .HasColumnName("descricao");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
