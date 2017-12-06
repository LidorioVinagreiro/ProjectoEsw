using ProjectoEsw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectoEsw.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace ProjectoEsw.GestorAplicacao
{
    public class Gestor : IGestor
    {
        private AplicacaoDbContexto _context;
        private UserManager<AplicacaoUtilizador> _userManager;
        private SignInManager<AplicacaoUtilizador> _signInManager;

        public Gestor(AplicacaoDbContexto context,UserManager<AplicacaoUtilizador> userManager, SignInManager<AplicacaoUtilizador> signInManager) {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }


    public async Task<int> adicionarPerfilAsync(RegisterViewModel model)
        {
        Perfil p = new Perfil {NomeCompleto = model.NomeCompleto };
            await _context.Perfils.AddAsync(p);
            _context.SaveChanges();
            return p.ID;  
        }
    }
}
