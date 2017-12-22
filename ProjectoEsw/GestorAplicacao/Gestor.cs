using ProjectoEsw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectoEsw.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ProjectoEsw.Models.ViewModels;
using ProjectoEsw.Models.Calendario;

namespace ProjectoEsw.GestorAplicacao
{

    /*public enum Roles { Candidato, Tecnico, Administrador };*/
    
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
        
        public async Task<string> getUtilizadorRole(string email)
        {
            Utilizador user = await _userManager.FindByNameAsync(email);
            IList<string> listaRoles = await _userManager.GetRolesAsync(user);
            string role = listaRoles.FirstOrDefault();
            return role;
        }

        public async Task LogOut() {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> atribuirRoleUtilizador(Utilizador utilizador, string role) {
            IdentityResult result = await _userManager.AddToRoleAsync(utilizador, role);
            return result;
        }

        public async Task<int> criarPerfilUtilizador(RegisterViewModel model,string utilizadorFK) {
            Perfil perfil = new Perfil
            {
                NomeCompleto = model.NomeCompleto,
                DataNasc = model.DataNasc,
                Email = model.Email,
                Nacionalidade = model.Nacionalidade,
                Genero = model.Genero,
                MoradaRua=model.MoradaRua,
                MoradaDistrito=model.MoradaDistrito,
                MoradaConcelho = model.MoradaConcelho,
                MoradaCodigoPostal=model.MoradaCodigoPostal,
                Nif=model.Nif,
                NumeroIdentificacao=model.NumeroIdentificacao,
                Telefone=model.Telefone,
                UtilizadorFK=utilizadorFK
            };
            var rsult = await _context.Perfils.AddAsync(perfil);
            await _context.SaveChangesAsync();
            return perfil.ID;
        }

        public async Task<bool> EditarPerfilUtilizador(RegisterViewModel model) {

            var perfil = _context.Perfils.Where(utilizadorPerfil => utilizadorPerfil.Email == model.Email).SingleOrDefault();
            if (perfil != null)
            {
                perfil.Telefone = model.Telefone;
                perfil.MoradaCodigoPostal = model.MoradaCodigoPostal;
                perfil.MoradaConcelho = model.MoradaConcelho;
                perfil.MoradaDistrito = model.MoradaDistrito;
                perfil.NumeroIdentificacao = model.NumeroIdentificacao;
                perfil.Nif = model.Nif;
                await _context.SaveChangesAsync();
                return true;
            }
            else {
                //erro nao existe perfil
                return false;

            }
            
        }

        public async Task<bool> EditarPassword(RegisterViewModel model) {
            var perfil = _context.Perfils.Where(utilizadorPerfil => utilizadorPerfil.Email == model.Email).SingleOrDefault();
            var utilizador = _context.Users.Where(uti => uti.Email == model.Email).FirstOrDefault();
            await _userManager.RemovePasswordAsync(utilizador);
            await _userManager.ChangePasswordAsync(utilizador, "", model.Password);
            return true;
        }


        public async Task adicionarInfo() {
            Utilizador tec1 = new Utilizador { UserName = "tecnico1@est.pt" };
            Utilizador tec2 = new Utilizador { UserName = "tecnico2@est.pt" };
            Utilizador tec3 = new Utilizador { UserName = "tecnico3@est.pt" };
            Utilizador admin = new Utilizador { UserName = "admin@est.pt" };

            try
            {
                IdentityResult a1 = await _userManager.CreateAsync(tec1, "tecnico1");
                IdentityResult a2 = await _userManager.CreateAsync(tec2, "tecnico2");
                IdentityResult a3 = await _userManager.CreateAsync(tec3, "tecnico3");
                IdentityResult a4 = await _userManager.CreateAsync(admin, "admin");
                await _context.SaveChangesAsync();

                IdentityResult a5 = await _userManager.AddToRoleAsync(tec1, "Tecnico");
                IdentityResult a6 = await _userManager.AddToRoleAsync(tec2, "Tecnico");
                IdentityResult a7 = await _userManager.AddToRoleAsync(tec3, "Tecnico");
                IdentityResult a8 = await _userManager.AddToRoleAsync(admin, "Administrador");
                await _context.SaveChangesAsync();
            }
            catch (Exception e) {
                //nai faz nada aqui
                Console.WriteLine(e.ToString());
            }
        }

        public async Task<Utilizador> getUtilizador(ClaimsPrincipal principal)
        {
            return await _userManager.GetUserAsync(principal);
        }


             public Perfil getPerfil(Utilizador user)
        {
            Perfil queryPerfil = (from perfil in _context.Perfils
                                  where perfil.ID == user.PerfilFK
                                  select new Perfil
                                  {
                                      ID = perfil.ID,  
                                      NomeCompleto = perfil.NomeCompleto,
                                      Email = perfil.Email,
                                      MoradaRua = perfil.MoradaRua,
                                      MoradaDistrito = perfil.MoradaDistrito,
                                      MoradaConcelho = perfil.MoradaConcelho,
                                      MoradaCodigoPostal = perfil.MoradaCodigoPostal,
                                      Nacionalidade = perfil.Nacionalidade,
                                      Genero = perfil.Genero,
                                      NumeroIdentificacao = perfil.NumeroIdentificacao,
                                      DataNasc = perfil.DataNasc,
                                      Nif = perfil.Nif,
                                      Telefone = perfil.Telefone
                                  }).FirstOrDefault();
            return queryPerfil;
        }
        public async Task<Utilizador> getUtilizadorByEmail(string email) {
            Utilizador user = await _userManager.FindByNameAsync(email);
            return user;
        }
        /* nao sei se faz falta*/
        public List<Eventos> getEventos(Perfil perfil) {
            return _context.Eventos.Select(even => even).Where(even => even.PerfilFK == perfil.ID).ToList();
        }
    }
}
