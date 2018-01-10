using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ProjectoEsw.Models.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectoEsw.Models.Ajudas;
using ProjectoEsw.Models.Calendario;
using ProjectoEsw.Models.Candidatura_sprint2;

namespace ProjectoEsw.Models
{
    public class AplicacaoDbContexto : IdentityDbContext<Utilizador>
    {
        public AplicacaoDbContexto(DbContextOptions<AplicacaoDbContexto> options) : base(options) { }
        public DbSet<Perfil> Perfils { get; set; }
        public DbSet<AjudaCampo> AjudaCampos { get; set; }
        public DbSet<AjudaPagina> AjudaPaginas { get; set; }
        public DbSet<Eventos> Eventos { get; set; }
        public DbSet<Candidatura> Candidaturas { get; set; }
        public DbSet<TipoCandidatura> TipoCandidatuas { get; set; }

        public DbSet<Instituicao> Instituicoes { get; set; }

        public DbSet<Instituicoes_Candidatura> InstituicoesCandidatura { get; set; }
        //public DbSet<Perfil> Perfils { get; set; }
        //este é a classe da base de dados
        //este serviço tem que ser adicionado no startup
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Instituicoes_Candidatura>().
                HasKey(classe => new { classe.CandidaturaId, classe.InstituicaoId });

            builder.Entity<Instituicoes_Candidatura>().
                HasOne(_candidatura => _candidatura.Candidatura).
                WithMany(_candidatura => _candidatura.Instituicoes).
                HasForeignKey(_candidatura => _candidatura.CandidaturaId);

            builder.Entity<Instituicoes_Candidatura>().
                HasOne(_instituicao => _instituicao.Instituicao).
                WithMany(_instituicao => _instituicao.Candidaturas).
                HasForeignKey(_instituicao => _instituicao.InstituicaoId);
        }
    }
}
