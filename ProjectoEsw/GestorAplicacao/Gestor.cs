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

        public Gestor(AplicacaoDbContexto context,
            UserManager<Utilizador> userManager,
            SignInManager<Utilizador> signInManager) {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> criarUtilizador(RegisterViewModel model) {
            Utilizador novoUtilizador = new Utilizador { UserName = model.Email };
            novoUtilizador.PerfilFK = await criarPerfilUtilizador(model, "0");
            IdentityResult resultado = await _userManager.CreateAsync(novoUtilizador, model.Password);
            if (resultado.Succeeded)
            {
                IdentityResult resultadoRole = await atribuirRoleUtilizador(novoUtilizador, "Candidato");
                if (resultadoRole.Succeeded)
                {               
                    await _context.SaveChangesAsync();
                    LoginViewModel loginModel = new LoginViewModel { Email = model.Email, Password = model.Password };
                    SignInResult autenticaResultado = await autenticarUtilizador(loginModel);
                    return autenticaResultado.Succeeded;
                }
                else {
                    //nao existe o role
                    //nao foi atribuido o role
                    return false;
                }
            } else {
                //nao foi criado o utilizador
                return false;
            }
        }

        public async Task<SignInResult> autenticarUtilizador(LoginViewModel model) {
            SignInResult resultado = await _signInManager.PasswordSignInAsync(model.Email, model.Password,false,false);
            return resultado;
        }

        public async Task<IdentityResult> atribuirRoleUtilizador(Utilizador utilizador, string role) {
            return await _userManager.AddToRoleAsync(utilizador, role);
            
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

            var rsult = await _context.Perfils.AddAsync(perfil);

            return perfil.ID;

        }
        public async Task<bool> EditarPerfilUtilizador(RegisterViewModel model) {
            //quais?
            
            return true;
        }
        
    }
}
