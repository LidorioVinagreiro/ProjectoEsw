using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProjectoEsw.Models.Identity;

namespace ProjectoEsw.Models
{
    public static class DbInitializer
    {
        public static void Initialize(AplicacaoDbContexto context)
        {
            if (!context.Eventos.Any()) {
                context.Eventos.Add(new Calendario.Eventos {
                    Inicio = new DateTime(2000, 10, 10),
                    Fim = new DateTime(2000, 10, 11),
                    Descricao = "DESCRICAO",
                    Titulo="TITULO",
                    DiaTodo=false
                    }    
                );

            }
            if (!context.Roles.Any())
            {
                context.Roles.Add(new IdentityRole { Name = "Candidato", NormalizedName = "CANDIDATO" });
                context.Roles.Add(new IdentityRole { Name = "Tecnico", NormalizedName = "TECNICO" });
                context.Roles.Add(new IdentityRole { Name="Administrador", NormalizedName="ADMINISTRADOR"});
                context.SaveChanges();
            }


            
        }
    }
}

