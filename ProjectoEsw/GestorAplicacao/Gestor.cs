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
using ProjectoEsw.Models.Candidatura_sprint2;
using ProjectoEsw.Models.Candidatura_sprint2.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ProjectoEsw.GestorAplicacao
{
    
    public class Gestor
    {
        private AplicacaoDbContexto _context;
        private UserManager<Utilizador> _userManager;
        private SignInManager<Utilizador> _signInManager;
        private GestorEmail _gestorEmail;
        public Gestor(AplicacaoDbContexto context,
            UserManager<Utilizador> userManager,
            SignInManager<Utilizador> signInManager,
            GestorEmail gestorEmail) {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _gestorEmail = gestorEmail;
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

        public async Task<bool> EditarPerfilUtilizador(RegisterViewModel model, string email) {

            var perfil = _context.Perfils.Where(utilizadorPerfil => utilizadorPerfil.Email == email).SingleOrDefault();
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

        public async Task<bool> adicionarCandidatura(Utilizador user, Candidatura candidatura,CandidaturaViewModel model)
        {
            try
            {
                await _context.Candidaturas.AddAsync(candidatura);
                await _context.SaveChangesAsync();
                List<Instituicoes_Candidatura> instituicoes = new List<Instituicoes_Candidatura>();
                for (int i = 0; i < model.Instituicoes.Count; i++)
                {
                    instituicoes.Add(new Instituicoes_Candidatura
                    {
                        CandidaturaId = candidatura.ID,
                        InstituicaoId = model.Instituicoes[i].ID

                    });
                }
                _context.Instituicoes_Candidatura.AddRange(instituicoes);
                await _context.SaveChangesAsync();
                return true;
            }   
            catch (Exception e) {
                e.ToString();
                return false;
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
                                  }).Include(x => x.Utilizador).FirstOrDefault();
            return queryPerfil;
        }
        public async Task<Utilizador> getUtilizadorByEmail(string email) {
            Utilizador user = await _userManager.FindByNameAsync(email);
            return user;
        }

        public List<Eventos> getEventos(Perfil perfil) {
            return _context.Eventos.Select(even => even).Where(even => even.PerfilFK == perfil.ID).ToList();
        }

        public List<SelectListItem> InstituicoesInternasSelect() {
            return _context.Instituicoes.Where(row => row.Interno == true).Select(x => new SelectListItem { Text = x.NomeInstituicao, Value = x.NomeInstituicao }).ToList();

        }
        public List<Instituicao> InstituicoesInternas() {
            return _context.Instituicoes.Where(x => x.Interno == true).ToList();
        }
        public List<Instituicao> InstituicoesExternas()
        {
            return _context.Instituicoes.Where(x => x.Interno == false).ToList();
        }

        public bool RejeitarCandidatura(Candidatura candidatura,Utilizador Tecnico) {
            if (!candidatura.Estado.Equals(Estado.EM_ANALISE))
                return false;
            candidatura.Estado = Estado.REJEITADA;
            _context.SaveChanges();
            _gestorEmail.EnviarEmail(candidatura.Candidato, "Candidatura rejeitada", candidatura.ToString());
            return true;            
        }
        public bool AprovarCandidatura(Candidatura candidatura, Utilizador Tecnico)
        {
            if (!candidatura.Estado.Equals(Estado.EM_ANALISE))
                return false;
            candidatura.Estado = Estado.APROVADA;
            _context.SaveChanges();
            _gestorEmail.EnviarEmail(candidatura.Candidato, "Candidatura aprovada", candidatura.ToString());
            return true;
        }
        public bool PedirAlteracaoCandidatura(Candidatura candidatura, Utilizador Tecnico)
        {
            if (!candidatura.Estado.Equals(Estado.EM_ANALISE))
                return false;
            candidatura.Estado = Estado.INCOMPLETA;
            _context.SaveChanges();
            _gestorEmail.EnviarEmail(candidatura.Candidato, "Candidatura necessita de alteracao", candidatura.ToString());
            return true;
        }

    }
}
