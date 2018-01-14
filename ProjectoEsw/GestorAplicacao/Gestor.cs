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
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using ProjectoEsw.Models.Estatisticas_sprint3.ViewModel;

namespace ProjectoEsw.GestorAplicacao
{
    
    public class Gestor
    {
        private AplicacaoDbContexto _context;
        private UserManager<Utilizador> _userManager;
        private SignInManager<Utilizador> _signInManager;
        private GestorEmail _gestorEmail;
        private IHostingEnvironment _hostingEnvironment;
        private string DirectoriaUtilizadores = "\\Utilizadores";
        private string DirectoriaDocumentos = "\\Documentos";
        private string DirectoriaImagem = "\\Imagem";

        public Gestor(AplicacaoDbContexto context,
            UserManager<Utilizador> userManager,
            SignInManager<Utilizador> signInManager,
            GestorEmail gestorEmail,
            IHostingEnvironment _environment) {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _gestorEmail = gestorEmail;
            _hostingEnvironment = _environment;
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
            this.GerarDirectoriaUtilizador(perfil);
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



        public bool GerarDirectoriaUtilizador(Perfil user) {
            string users = DirectoriaUtilizadores + user.Email;
            string documentos = users+ DirectoriaDocumentos;
            string imagem = users + DirectoriaImagem;
            string path = _hostingEnvironment.WebRootPath + documentos;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                string path1 = _hostingEnvironment.WebRootPath + imagem;
                Directory.CreateDirectory(path1);
                return true;
            
             }
            
            return false;
        }

        public async Task<bool> UploadFotoUtilizador(Utilizador user,IFormFile imagem) {
            if (imagem == null || imagem.Length == 0)
                return false;

            var pathImagem = DirectoriaUtilizadores + "\\" + user.Email + DirectoriaImagem;
            var path = Path.Combine(
                        _hostingEnvironment.WebRootPath + pathImagem, imagem.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await imagem.CopyToAsync(stream);
            }
            _context.Perfils.Where(row => row.UtilizadorFK == user.Id).Single().Foto = imagem.FileName;
            _context.SaveChanges();
            return true;
        }

        public string getImagePath(Utilizador user) {
            var pathImagem = DirectoriaUtilizadores + "\\" + user.Email + DirectoriaImagem;
            Perfil perfil =_context.Perfils.Where(row => row.UtilizadorFK == user.Id).Single();
            return Path.Combine( _hostingEnvironment.WebRootPath , pathImagem , perfil.Foto);
        }

        public Utilizador getUtilizadorByPerfilId(int idPerfil) {
            return _context.Users.Where(row => row.PerfilFK == idPerfil).Single();

        }

        public Utilizador getUtilizadorById(string id)
        {
            return _context.Users.Where(row => row.Id == id).Single();

        }
        public bool MarcarEntrevista(Utilizador tecnico,Utilizador candidato,DateTime inicio,DateTime fim) {
            Candidatura candidatura = _context.Candidaturas.Where(row => row.UtilizadorFK == candidato.Id).Single();
            if (!candidatura.Estado.Equals(Estado.APROVADA))
                return false;
            Perfil tec = _context.Perfils.Where(row => row.UtilizadorFK == tecnico.Id).Single();
            Eventos evento = new Eventos { EntrevistadorFK = tec.ID ,Tipo = Tipo.Entrevista,Descricao = "Entrevista : " + candidato.UserName ,Inicio =inicio, Fim= fim,Titulo="Entrevista a Candidato"};
            _context.SaveChanges();
            return true;
        }

        public bool MarcarReuniao(Utilizador candidato,DateTime inicio,DateTime fim)
        {
            Perfil candidatop = _context.Perfils.Where(row => row.UtilizadorFK == candidato.Id).Single();
            Eventos evento = new Eventos { PerfilFK = candidatop.ID, Inicio = inicio, Fim = fim , Titulo="REUNIAO NA ASSOCIACAO",Descricao="Duvidas na Candidatura" };
            _context.SaveChanges();
            string tecnico = "tecnico";
            IdentityRole tecn = _context.Roles.Where(row => row.Name.ToUpper().Equals(tecnico.ToUpper())).Single();
            List<IdentityUserRole<string>> users = _context.UserRoles.Where(row => row.RoleId == tecn.Id).ToList();
            List<Utilizador> usersList = _context.Users.Where(row => users.Select(row1 => row1.UserId).Contains(row.Id)).ToList();
            foreach (var item in usersList)
            {
                _gestorEmail.EnviarEmail(item, "FOI MARCADA UMA REUNIAO", inicio.ToString());
            }
            
            return true;
        }

        public EstatisticasGerais GerarEstatisticas() {
            return new EstatisticasGerais { };
        }
    }
}
