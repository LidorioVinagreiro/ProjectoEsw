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
        private UserManager<Utilizador> _userManager;
        private SignInManager<Utilizador> _signInManager;

        public Gestor(AplicacaoDbContexto context,UserManager<Utilizador> userManager, SignInManager<Utilizador> signInManager) {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> criarUtilizador(RegisterViewModel model) {
            Utilizador novoUtilizador = new Utilizador { UserName = model.Email };
            IdentityResult resultado = await _userManager.CreateAsync(novoUtilizador, model.Password);
            if (resultado.Succeeded)
            {
                var resultadoPerfil = await criarPerfilUtilizador(model, novoUtilizador.Id);
                novoUtilizador.PerfilFK = resultadoPerfil;
                await _context.SaveChangesAsync();
                LoginViewModel loginModel = new LoginViewModel { Email = model.Email, Password = model.Password };
                return await autenticarUtilizador(loginModel);
            }
            else {
                //adicionar erros?
                return false;
            }
        }

        public async Task<bool> autenticarUtilizador(LoginViewModel model) {
            var resultado = await _signInManager.PasswordSignInAsync(model.Email, model.Password,false,false);
            if (resultado.Succeeded)
            {
                //autenticado
                return true;
            }
            else {
                //nao autenticado
                return false;

            }
        }

        public async Task<int> criarPerfilUtilizador(RegisterViewModel model,string utilizadorFK) {
            Perfil perfil = new Perfil
            {
              NomeCompleto =model.NomeCompleto,
              DataNasc=model.DataNasc,
              Email=model.Email,
              Morada=model.Morada,
              Nif=model.Nif,
              NumeroIdentificacao=model.NumeroIdentificacao,
              Telefone=model.Telefone,
              UtilizadorFK=utilizadorFK
            };

            await _context.Perfils.AddAsync(perfil);
            return perfil.ID;

        }
        public async Task<bool> EditarPerfilUtilizador(RegisterViewModel model) {
            //quais?
            
            return true;
        }
        
    }
}
