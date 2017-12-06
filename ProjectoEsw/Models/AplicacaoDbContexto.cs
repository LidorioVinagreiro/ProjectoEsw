using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ProjectoEsw.Models.Identity;
using Microsoft.EntityFrameworkCore;

namespace ProjectoEsw.Models
{
    public class AplicacaoDbContexto : IdentityDbContext<AplicacaoUtilizador>
    {
        public AplicacaoDbContexto(DbContextOptions<AplicacaoDbContexto> options) : base(options) { }
        public DbSet<Perfil> Perfils { get; set; } 
        //public DbSet<Perfil> Perfils { get; set; }
        //este é a classe da base de dados
        //este serviço tem que ser adicionado no startup
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Perfil>().ToTable("Perfil");
            
        }
    }
}
