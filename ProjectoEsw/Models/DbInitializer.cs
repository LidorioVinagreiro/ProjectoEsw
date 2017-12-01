using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ProjectoEsw.Models
{
    public static class DbInitializer
    {

        public static void Initialize(AplicacaoDbContexto context)
        {
            context.Roles.Add(new IdentityRole("Candidato"));
            context.SaveChanges();
        }
    }
}
