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
using Microsoft.Extensions.Primitives;

namespace ProjectoEsw.GestorAplicacao
{
    /// <summary>
    /// Esta classe é responsavel por tarefas de controlo/registo de operações de bussiness intelligence
    /// </summary>
    public class Gestor
    {
        /// <summary>
        /// Atributos privados da classe Gestor
        /// </summary>
        private AplicacaoDbContexto _context;
        private UserManager<Utilizador> _userManager;
        private SignInManager<Utilizador> _signInManager;
        private GestorEmail _gestorEmail;
        private IHostingEnvironment _hostingEnvironment;
        private string DirectoriaUtilizadores = "\\Utilizadores";
        private string DirectoriaDocumentos = "\\Documentos";
        private string DirectoriaImagem = "\\Imagem";
        /// <summary>
        /// Metodo construtor da classe Gestor, todos os parametros são recebidos atraves do DI(dependency injection)
        /// </summary>
        /// <param name="context">Parametro do tipo ApplicationDbContexto</param>
        /// <param name="userManager">Parametro do tipo UserManager<Utilizador></param>
        /// <param name="signInManager">Parametro do tipo SignInManager<Utilizador></param>
        /// <param name="gestorEmail">Parametro do tipo GestorEmail</param>
        /// <param name="_environment">Parametro do tipo IHostingEnvironment</param>
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

        /// <summary>
        /// Esta função tem como objectivo devolver um Role de um determinado utilizador
        /// </summary>
        /// <param name="email">Parametro do tipo string que representa o Email do utilizador</param>
        /// <returns>Retorna uma função assincrona do tipo string(Task<string>) sendo o seu resultado o valor do Role</returns>
        public async Task<string> getUtilizadorRole(string email)
        {
            Utilizador user = await _userManager.FindByNameAsync(email);
            IList<string> listaRoles = await _userManager.GetRolesAsync(user);
            string role = listaRoles.FirstOrDefault();
            return role;
        }
        /// <summary>
        /// Esta função tem como objectivo fazer log out do utilizador
        /// </summary>
        /// <returns></returns>
        public async Task LogOut() {
            await _signInManager.SignOutAsync();
        }
        /// <summary>
        /// Esta função tem como objectivo registar um determinado Role a um Utilizador
        /// </summary>
        /// <param name="utilizador">Parametro do tipo Utilizador</param>
        /// <param name="role">Parametro do tipo string</param>
        /// <returns>Retorna um Task<IndentifyResult> que permite saber se a função foi efectuada com sucesso ou não</returns>
        public async Task<IdentityResult> atribuirRoleUtilizador(Utilizador utilizador, string role) {
            IdentityResult result = await _userManager.AddToRoleAsync(utilizador, role);
            return result;
        }
        /// <summary>
        /// Esta função tem como objectivo registar um perfil para um utilizador com uma determinada 
        /// informação na base de dados, bem como gerar as pastas para o mesmo
        /// </summary>
        /// <param name="model">Parametro do tipo RegisterViewModel que contém informações do utilizador</param>
        /// <param name="utilizadorFK">Parametro do tipo string que é o ID do utilizador</param>
        /// <returns>Retona uma task<int>, o valor inteiro é o id do Perfil gerado </returns>
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
        /// <summary>
        /// Esta função tem como objectivo editar o perfil de um determinado utilizador com novas informações
        /// </summary>
        /// <param name="model">Parametro do tipo RegisterViewModel, que contem a informação a ser alterada</param>
        /// <param name="email">Parametro do tipo string que remete a um determinado Utilizador</param>
        /// <returns>retorna uma task<bool> que se o resuldado for true significa que a operação foi efectuada com sucesso</returns>
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
        /// <summary>
        /// Esta função tem como objectivo de alterar a password de um determinado utilizador
        /// </summary>
        /// <param name="model">Parametro do tipo RegisterViewModel, com novas informações</param>
        /// <returns></returns>
        public async Task<bool> EditarPassword(RegisterViewModel model) {
            var perfil = _context.Perfils.Where(utilizadorPerfil => utilizadorPerfil.Email == model.Email).SingleOrDefault();
            var utilizador = _context.Users.Where(uti => uti.Email == model.Email).FirstOrDefault();
            await _userManager.RemovePasswordAsync(utilizador);
            await _userManager.ChangePasswordAsync(utilizador, "", model.Password);
            return true;
        }
        /// <summary>
        /// Esta função tem como objectivo de registar uma candidatura para um determinado utilizador
        /// </summary>
        /// <param name="user">Parametro do tipo Utilizador</param>
        /// <param name="candidatura">Parametro do tipo Candidatura</param>
        /// <param name="model"> Parametro do tipo CandidaturaViewModel que contem informação de um candidatura</param>
        /// <returns>O resultado desta tarefa é true se a candidatura for registada com sucesso</returns>
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
        /// <summary>
        /// Esta função tem como objectivo registar a alteração da candidatura do utilizador
        /// </summary>
        /// <param name="user">Parametro do tipo Utilizador</param>
        /// <param name="candidatura">Parametro do tipo Candidatura</param>
        /// <param name="model">Parametro do tipo CandidaturaViewModel</param>
        /// <returns></returns>
        public async Task<bool> AlterarCandidatura(Utilizador user, Candidatura candidatura, CandidaturaViewModel model) {
            List<Instituicoes_Candidatura> lista = _context.Instituicoes_Candidatura.Where(row => row.CandidaturaId == candidatura.ID).ToList();
            _context.Instituicoes_Candidatura.RemoveRange(lista);
            await _context.SaveChangesAsync();
            try
            {
                List<Instituicoes_Candidatura> instituicoes = new List<Instituicoes_Candidatura>();
                for (int i = 0; i < model.Instituicoes.Count; i++)
                {
                    instituicoes.Add(new Instituicoes_Candidatura
                    {
                        CandidaturaId = candidatura.ID,
                        InstituicaoId = model.Instituicoes[i].ID

                    });
                }
               
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                e.ToString();
                return false;
            }

        }
        /// <summary>
        /// Esta função tem como objectivo procura um utilizador atraves da sua Claim
        /// </summary>
        /// <param name="principal">Parametro do tipo ClaimsPrincipal</param>
        /// <returns>Retorna uma Task<Utilizador></returns>
        public async Task<Utilizador> getUtilizador(ClaimsPrincipal principal)
        {
            return await _userManager.GetUserAsync(principal);
        }

        /// <summary>
        /// Esta função tem como objectivo retornar um Perfil
        /// </summary>
        /// <param name="user">Parametro do tipo Utilizador</param>
        /// <returns>Retorna o Perfil de um determinado Utilizador</returns>
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
        /// <summary>
        /// Esta função tem como objectivo buscar um utilizador através do campo email
        /// </summary>
        /// <param name="email">Parametro do tipo string</param>
        /// <returns>Retorna uma tarefa assincrona (Task<Utilizador>)</returns>
        public async Task<Utilizador> getUtilizadorByEmail(string email) {
            Utilizador user = await _userManager.FindByNameAsync(email);
            return user;
        }
        /// <summary>
        /// Esta função tem como objectivo procurar uma lista de eventos de um determinado perfil
        /// </summary>
        /// <param name="perfil">Parametro do tipo Perfil, que remete a um utilizador</param>
        /// <returns>Retona uma List<Eventos> de um determinado utilizador</returns>
        public List<Eventos> getEventos(Perfil perfil) {
            return _context.Eventos.Select(even => even).Where(even => even.PerfilFK == perfil.ID).ToList();
        }
        /// <summary>
        /// Esta função tem como objectivo criar uma lista de SelectListItem de instituições com o campo
        /// "Interno" a true
        /// </summary>
        /// <returns>Retorna uma List<SelectListItem></returns>
        public List<SelectListItem> InstituicoesInternasSelect() {
            return _context.Instituicoes.Where(row => row.Interno == true).Select(x => new SelectListItem { Text = x.NomeInstituicao, Value = x.NomeInstituicao }).ToList();

        }
        /// <summary>
        /// Esta função tem como objectivo procurar todas as instituições que tem o campo "interno" a true
        /// </summary>
        /// <returns></returns>
        public List<Instituicao> InstituicoesInternas() {
            return _context.Instituicoes.Where(x => x.Interno == true).ToList();
        }
        /// <summary>
        /// Esta função tem como objectivo procurar todas as instituições que tem o campo "interno" a false
        /// </summary>
        /// <returns>Retorna uma lista de instituições</returns>
        public List<Instituicao> InstituicoesExternas()
        {
            return _context.Instituicoes.Where(x => x.Interno == false).ToList();
        }
        /// <summary>
        /// Esta função tem como objectivo registar a rejeição de uma candidatura
        /// </summary>
        /// <param name="candidatura">Candidatura de um utilizador</param>
        /// <param name="Tecnico">Parametro do tipo Utilizador com Role Tecnico</param>
        /// <returns>Retorna true se foi efectuado com sucesso</returns>
        public bool RejeitarCandidatura(Candidatura candidatura,Utilizador Tecnico) {
            if (!candidatura.Estado.Equals(Estado.EM_ANALISE))
                return false;
            candidatura.Estado = Estado.REJEITADA;
            _context.SaveChanges();
            _gestorEmail.EnviarEmail(candidatura.Candidato, "Candidatura rejeitada", candidatura.ToString());
            return true;            
        }
        /// <summary>
        /// Esta função tem como objectivo aprovar uma candidatura
        /// Esta função é utilizada por utilizador de Role tecnico
        /// </summary>
        /// <param name="candidatura">Candidatura de um utilizador</param>
        /// <param name="Tecnico">utilizador com Role do tipo Tecnico</param>
        /// <returns>Retorna true se a candidatura foi aprovada</returns>
        public bool AprovarCandidatura(Candidatura candidatura, Utilizador Tecnico)
        {
            if (!candidatura.Estado.Equals(Estado.EM_ANALISE))
                return false;
            candidatura.Estado = Estado.APROVADA;
            _context.SaveChanges();
            _gestorEmail.EnviarEmail(candidatura.Candidato, "Candidatura aprovada", candidatura.ToString());
            return true;
        }
        /// <summary>
        /// Esta função regista o pedido de alteração de uma candidatura por parte do tecnico
        /// </summary>
        /// <param name="candidatura">Candidatura de um determinado utilizador</param>
        /// <param name="Tecnico">Tecnico resposavel pelo pedido de alteração</param>
        /// <param name="propriedadesParaAlterar">Parametro do tipo StringValues que contem os campos que precisam de alteração</param>
        /// <returns>Retorna true se o pedido de alteração foi registado</returns>
        public bool PedirAlteracaoCandidatura(Candidatura candidatura, Utilizador Tecnico,StringValues propriedadesParaAlterar)
        {
            string emailBody = "";
            for (int i = 0; i < propriedadesParaAlterar.Count; i++)
            {
                emailBody += propriedadesParaAlterar.ToList()[i].ToString();
            }
            if (!candidatura.Estado.Equals(Estado.EM_ANALISE))
                return false;
            candidatura.Estado = Estado.INCOMPLETA;
            _context.SaveChanges();
            _gestorEmail.EnviarEmail(candidatura.Candidato, "Candidatura necessita de alteracao", emailBody);
            return true;
        }


        /// <summary>
        /// Esta função gera as directorias necessarias no servidor para um utilizador
        /// </summary>
        /// <param name="user">Parametro do tipo Perfil</param>
        /// <returns>Retorna true se as pastas foram criadas</returns>
        public bool GerarDirectoriaUtilizador(Perfil user) {
            string users = DirectoriaUtilizadores +"\\"+ user.Email;
            string documentos = users + DirectoriaDocumentos;
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
        /// <summary>
        /// Esta função faz o upload de uma imagem
        /// </summary>
        /// <param name="user">Parametro do tipo Utilizador</param>
        /// <param name="imagem">Parametro do tipo IFormFile que contem a imagem</param>
        /// <returns>Retorna true se o upload da imagem foi feito com sucesso</returns>
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
        /// <summary>
        /// Esta função devolve um caminho de uma imagem de um utilizador
        /// </summary>
        /// <param name="user"> Parametro do tipo Utilizador</param>
        /// <returns>Retorna o caminho da imagem do utilizador</returns>
        public string getImagePath(Utilizador user) {
            var pathImagem = DirectoriaUtilizadores + "\\" + user.Email + DirectoriaImagem;
            Perfil perfil =_context.Perfils.Where(row => row.UtilizadorFK == user.Id).Single();
            return Path.Combine( _hostingEnvironment.WebRootPath , pathImagem , perfil.Foto);
        }
        /// <summary>
        /// Esta função pretende procurar um utilizador pelo ID do Perfil
        /// </summary>
        /// <param name="idPerfil">Parametro do tipo int</param>
        /// <returns>Retorna um Utilizador(classe Utilizador)</returns>
        public Utilizador getUtilizadorByPerfilId(int idPerfil) {
            return _context.Users.Where(row => row.PerfilFK == idPerfil).Single();

        }
        /// <summary>
        /// Esta função retorna o utilizador pelo seu ID
        /// </summary>
        /// <param name="id">Parametro do tipo string(id)</param>
        /// <returns>Retorna um Utilizdor(classe Utilizador)</returns>
        public Utilizador getUtilizadorById(string id)
        {
            return _context.Users.Where(row => row.Id == id).Single();

        }

        /// <summary>
        /// Esta função tem como objectivo de marcar uma entrevista entre o utilizador(candidato) e outro utilizador(tecnico)
        /// </summary>
        /// <param name="tecnico">Parametro do tipo Utilizador</param>
        /// <param name="candidato">Parametro do tipo Utilizador</param>
        /// <param name="inicio">Parametro do tipo DateTime, sendo esta a data inicial da entrevista</param>
        /// <param name="fim">Parametro do tipo DateTime, sendo esta a data final da entrevista</param>
        /// <returns>Retorna true se a entrevista foi registada</returns>
        public bool MarcarEntrevista(Utilizador tecnico,Utilizador candidato,DateTime inicio,DateTime fim) {
            Candidatura candidatura = _context.Candidaturas.Where(row => row.UtilizadorFK == candidato.Id).Single();
            if (!candidatura.Estado.Equals(Estado.APROVADA))
                return false;
            Perfil tec = _context.Perfils.Where(row => row.UtilizadorFK == tecnico.Id).Single();
            Eventos evento = new Eventos { EntrevistadorFK = tec.ID ,Tipo = Tipo.Entrevista,Descricao = "Entrevista : " + candidato.UserName ,Inicio =inicio, Fim= fim,Titulo="Entrevista a Candidato"};
            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Esta função tem como objectivo de marcar uma reuniao entre o utilizador(candidato) e outro utilizador(tecnico).
        /// Esta função só pode ser usada por Utilizador com o Role ="Tecnico"
        /// </summary>
        /// <param name="candidato">Parametro do tipo Utilizador</param>
        /// <param name="inicio">Parametro do tipo DateTime, sendo esta a data inicial da reunião</param>
        /// <param name="fim">Parametro do tipo DateTime, sendo esta a data final da reunião</param>
        /// <returns>Retorna true se a reunião foi registada</returns>
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
        /// <summary>
        /// Esta função gera uma ViewModel(Estatisticas Gerais)
        /// Esta função é responsavel pelo calculo dos diferentes tipos de estatisticas
        /// </summary>
        /// <returns></returns>
        public EstatisticasGerais GerarEstatisticas() {
            EstatisticasGerais model = new EstatisticasGerais { };

            List<ARCandidaturasEstatisticas> ar = new List<ARCandidaturasEstatisticas>();
            foreach (string x in Enum.GetNames(Estado.APROVADA.GetType())) {
                Estado EstadoValue = (Estado)Enum.Parse(typeof(Estado), x);
                int  valor = _context.Candidaturas.Where(row => row.Estado.Equals(EstadoValue)).Count();
                ar.Add(new ARCandidaturasEstatisticas { Estado = x, intNCandidaturasEstado = valor });
            }

            List<TipoQuantidadeEstatistica> tqe = new List<TipoQuantidadeEstatistica>(); // Funciona!
            foreach (TipoCandidatura tipo in _context.TipoCandidatuas) {
                int valor = _context.Candidaturas.Where(row => row.TipoCandidaturaFK == tipo.ID).Count();
                tqe.Add(new TipoQuantidadeEstatistica { TipoMobilidade = tipo.Tipo, nCandidatos = valor });    
            }

            TotalBolsaEstatisticas bolsas = new TotalBolsaEstatisticas { };
            bolsas.QuantidadeBolsas = _context.Candidaturas.Where(row => row.Bolsa == true).Count();
            bolsas.QuantidadeSemBolsa = _context.Candidaturas.Where(row => row.Bolsa == false).Count();

            //mini barraca
            var query = _context.Instituicoes_Candidatura.Include(row => row.Instituicao).GroupBy(row => row.InstituicaoId);
            DestinosPreferenciasEstatisticas dpe = new DestinosPreferenciasEstatisticas { PreferenciaMaior = query.FirstOrDefault().Single().Instituicao, PreferenciaMenor = query.LastOrDefault().Single().Instituicao };

            List<InstituicaoEstatisticas> ie = new List<InstituicaoEstatisticas>();

            foreach (Instituicao x in _context.Instituicoes) {
                int valor = _context.Instituicoes_Candidatura.Where(row => row.InstituicaoId == x.ID).Count();
                ie.Add(new InstituicaoEstatisticas { Instituicao = x, QuantidadeAlunosInscritos = valor });
            }

            model.ARCandidaturaEstatisticas = ar;
            model.TipoQuantidadeEstatisticas = tqe;
            model.TotalBolsaEstatisticas = bolsas;
            model.DestinosPreferenciasEstatisticas = dpe;
            model.InstituicaoEstatistica = ie;
            return model;
        }
    }
}
