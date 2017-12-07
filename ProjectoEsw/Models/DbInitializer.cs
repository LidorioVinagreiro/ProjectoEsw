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
        public static void InitializeAsync(AplicacaoDbContexto context,UserManager<Utilizador> userManager)
        {
            if (!context.Roles.Any())
            {
                context.Roles.Add(new IdentityRole("Candidato"));
                context.Roles.Add(new IdentityRole("Tecnico"));
                context.Roles.Add(new IdentityRole("Administrador"));
                context.SaveChanges();
            }

            Utilizador tec1 = new Utilizador { Email = "tecnico1@est.pt" };
            Utilizador tec2 = new Utilizador { Email = "tecnico2@est.pt" };
            Utilizador tec3 = new Utilizador { Email = "tecnico3@est.pt" };
            Utilizador admin = new Utilizador { Email = "admin@est.pt" };

           
                userManager.CreateAsync(tec1, "tecnico1");
                userManager.CreateAsync(tec2, "tecnico2");
                userManager.CreateAsync(tec3, "tecnico3");
                userManager.CreateAsync(admin, "admin");

                userManager.AddToRoleAsync(tec1, "Tecnico");
                userManager.AddToRoleAsync(tec2, "Tecnico");
                userManager.AddToRoleAsync(tec3, "Tecnico");
                userManager.AddToRoleAsync(admin, "Administrador");
            context.SaveChanges();
        }
    }
}

