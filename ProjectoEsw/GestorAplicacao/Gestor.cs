using ProjectoEsw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectoEsw.Models.Identity;

namespace ProjectoEsw.GestorAplicacao
{
    public class Gestor : IGestor
    {
        private AplicacaoDbContexto context;
        public Gestor(AplicacaoDbContexto context) {
            this.context = context;
        }


    public async void adicionarPerfilAsync(RegisterViewModel model)
        {
        Perfil p = new Perfil {NomeCompleto = model.NomeCompleto };
            await context.Perfils.AddAsync(p);
            context.SaveChanges();
        }
    }
}
