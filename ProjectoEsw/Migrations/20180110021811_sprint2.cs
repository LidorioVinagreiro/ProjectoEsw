using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ProjectoEsw.Migrations
{
    public partial class sprint2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AjudaCampos",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Campo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaginaFK = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AjudaCampos", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AjudaPaginas",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pagina = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AjudaPaginas", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Instituicoes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Interno = table.Column<bool>(type: "bit", nullable: false),
                    LatitudeInstituicao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LongitudeInstituicao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomeInstituicao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaisInstituicao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteInstituicao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instituicoes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TipoCandidatuas",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoCandidatuas", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "Candidaturas",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AfiliacaoEmergencia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnoCurricular = table.Column<int>(type: "int", nullable: false),
                    Bolsa = table.Column<bool>(type: "bit", nullable: false),
                    CandidatoId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CartaMotivacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Escola = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    IBAN = table.Column<int>(type: "int", nullable: false),
                    NomeEmergencia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroEmergencia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoCandidaturaFK = table.Column<int>(type: "int", nullable: false),
                    TipoCandidaturaID = table.Column<int>(type: "int", nullable: true),
                    UtilizadorFK = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidaturas", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Candidaturas_TipoCandidatuas_TipoCandidaturaID",
                        column: x => x.TipoCandidaturaID,
                        principalTable: "TipoCandidatuas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Instituicoes_Candidatura",
                columns: table => new
                {
                    CandidaturaId = table.Column<int>(type: "int", nullable: false),
                    InstituicaoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instituicoes_Candidatura", x => new { x.CandidaturaId, x.InstituicaoId });
                    table.ForeignKey(
                        name: "FK_Instituicoes_Candidatura_Candidaturas_CandidaturaId",
                        column: x => x.CandidaturaId,
                        principalTable: "Candidaturas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Instituicoes_Candidatura_Instituicoes_InstituicaoId",
                        column: x => x.InstituicaoId,
                        principalTable: "Instituicoes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Perfils",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DataNasc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Genero = table.Column<int>(type: "int", nullable: false),
                    MoradaCodigoPostal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoradaConcelho = table.Column<int>(type: "int", nullable: false),
                    MoradaDistrito = table.Column<int>(type: "int", nullable: false),
                    MoradaRua = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nacionalidade = table.Column<int>(type: "int", nullable: false),
                    Nif = table.Column<int>(type: "int", nullable: false),
                    NomeCompleto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroIdentificacao = table.Column<int>(type: "int", nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UtilizadorFK = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perfils", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PerfilFK = table.Column<int>(type: "int", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Perfils_PerfilFK",
                        column: x => x.PerfilFK,
                        principalTable: "Perfils",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CorEvento = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaTodo = table.Column<bool>(type: "bit", nullable: false),
                    EntrevistadorFK = table.Column<int>(type: "int", nullable: true),
                    EntrevistadorID = table.Column<int>(type: "int", nullable: true),
                    Fim = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Inicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PerfilFK = table.Column<int>(type: "int", nullable: true),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UtilizadorID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Eventos_Perfils_EntrevistadorID",
                        column: x => x.EntrevistadorID,
                        principalTable: "Perfils",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Eventos_Perfils_UtilizadorID",
                        column: x => x.UtilizadorID,
                        principalTable: "Perfils",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PerfilFK",
                table: "AspNetUsers",
                column: "PerfilFK");

            migrationBuilder.CreateIndex(
                name: "IX_Candidaturas_CandidatoId",
                table: "Candidaturas",
                column: "CandidatoId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidaturas_TipoCandidaturaID",
                table: "Candidaturas",
                column: "TipoCandidaturaID");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_EntrevistadorID",
                table: "Eventos",
                column: "EntrevistadorID");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_UtilizadorID",
                table: "Eventos",
                column: "UtilizadorID");

            migrationBuilder.CreateIndex(
                name: "IX_Instituicoes_Candidatura_InstituicaoId",
                table: "Instituicoes_Candidatura",
                column: "InstituicaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Perfils_UtilizadorFK",
                table: "Perfils",
                column: "UtilizadorFK");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Candidaturas_AspNetUsers_CandidatoId",
                table: "Candidaturas",
                column: "CandidatoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Perfils_AspNetUsers_UtilizadorFK",
                table: "Perfils",
                column: "UtilizadorFK",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Perfils_AspNetUsers_UtilizadorFK",
                table: "Perfils");

            migrationBuilder.DropTable(
                name: "AjudaCampos");

            migrationBuilder.DropTable(
                name: "AjudaPaginas");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Eventos");

            migrationBuilder.DropTable(
                name: "Instituicoes_Candidatura");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Candidaturas");

            migrationBuilder.DropTable(
                name: "Instituicoes");

            migrationBuilder.DropTable(
                name: "TipoCandidatuas");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Perfils");
        }
    }
}
