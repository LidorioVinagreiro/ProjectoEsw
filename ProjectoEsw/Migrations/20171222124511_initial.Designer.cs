﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using ProjectoEsw.Models;
using ProjectoEsw.Models.Calendario;
using System;

namespace ProjectoEsw.Migrations
{
    [DbContext(typeof(AplicacaoDbContexto))]
    [Migration("20171222124511_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ProjectoEsw.Models.Ajudas.AjudaCampo", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Campo");

                    b.Property<string>("Descricao");

                    b.Property<int?>("PaginaFK");

                    b.HasKey("ID");

                    b.ToTable("AjudaCampos");
                });

            modelBuilder.Entity("ProjectoEsw.Models.Ajudas.AjudaPagina", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Descricao");

                    b.Property<string>("Pagina");

                    b.HasKey("ID");

                    b.ToTable("AjudaPaginas");
                });

            modelBuilder.Entity("ProjectoEsw.Models.Calendario.Eventos", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CorEvento");

                    b.Property<string>("Descricao");

                    b.Property<bool>("DiaTodo");

                    b.Property<int?>("EntrevistadorFK");

                    b.Property<int?>("EntrevistadorID");

                    b.Property<DateTime>("Fim");

                    b.Property<DateTime>("Inicio");

                    b.Property<int?>("PerfilFK");

                    b.Property<int>("Tipo");

                    b.Property<string>("Titulo");

                    b.Property<int?>("UtilizadorID");

                    b.HasKey("ID");

                    b.HasIndex("EntrevistadorID");

                    b.HasIndex("UtilizadorID");

                    b.ToTable("Eventos");
                });

            modelBuilder.Entity("ProjectoEsw.Models.Identity.Perfil", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DataNasc");

                    b.Property<string>("Email");

                    b.Property<string>("Foto");

                    b.Property<string>("Genero");

                    b.Property<string>("MoradaCodigoPostal");

                    b.Property<string>("MoradaConcelho");

                    b.Property<string>("MoradaDistrito");

                    b.Property<string>("MoradaRua");

                    b.Property<string>("Nacionalidade");

                    b.Property<int>("Nif");

                    b.Property<string>("NomeCompleto");

                    b.Property<int>("NumeroIdentificacao");

                    b.Property<string>("Telefone");

                    b.Property<string>("UtilizadorFK");

                    b.HasKey("ID");

                    b.HasIndex("UtilizadorFK");

                    b.ToTable("Perfils");
                });

            modelBuilder.Entity("ProjectoEsw.Models.Identity.Utilizador", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<int?>("PerfilFK");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("PerfilFK");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ProjectoEsw.Models.Identity.Utilizador")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ProjectoEsw.Models.Identity.Utilizador")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProjectoEsw.Models.Identity.Utilizador")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ProjectoEsw.Models.Identity.Utilizador")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProjectoEsw.Models.Calendario.Eventos", b =>
                {
                    b.HasOne("ProjectoEsw.Models.Identity.Perfil", "Entrevistador")
                        .WithMany()
                        .HasForeignKey("EntrevistadorID");

                    b.HasOne("ProjectoEsw.Models.Identity.Perfil", "Utilizador")
                        .WithMany()
                        .HasForeignKey("UtilizadorID");
                });

            modelBuilder.Entity("ProjectoEsw.Models.Identity.Perfil", b =>
                {
                    b.HasOne("ProjectoEsw.Models.Identity.Utilizador", "Utilizador")
                        .WithMany()
                        .HasForeignKey("UtilizadorFK");
                });

            modelBuilder.Entity("ProjectoEsw.Models.Identity.Utilizador", b =>
                {
                    b.HasOne("ProjectoEsw.Models.Identity.Perfil", "Perfil")
                        .WithMany()
                        .HasForeignKey("PerfilFK");
                });
#pragma warning restore 612, 618
        }
    }
}
