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
            if (!context.Roles.Any())
            {
                context.Roles.Add(new IdentityRole("Candidato"));
                context.Roles.Add(new IdentityRole("Tecnico"));
                context.Roles.Add(new IdentityRole("Administrador"));
                context.SaveChanges();
            }

            
        }
    }
}

