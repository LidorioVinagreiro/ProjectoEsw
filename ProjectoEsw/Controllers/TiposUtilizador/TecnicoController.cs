using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProjectoEsw.Models.Candidatura_sprint2;
using ProjectoEsw.Models;
using ProjectoEsw.GestorAplicacao;
using ProjectoEsw.Models.Identity;
using ProjectoEsw.Models.Candidatura_sprint2.ViewModels;
using Microsoft.Extensions.Primitives;
using Microsoft.EntityFrameworkCore;

namespace ProjectoEsw.Controllers
{
    /// <summary>
    /// Este controlador é somente acedido por utilizadores com o role Tecnico
    /// </summary>
    [Authorize(Roles = "Tecnico")]
    public class TecnicoController : Controller
    {
        /// <summary>
        /// Atributos privados do controlador
        /// </summary>
        private AplicacaoDbContexto _context;
        private Gestor _gestor;
        /// <summary>
        /// Construtor do controlador Tecnico, os parametros deste controlador são recebidos por DI(ddependency injection)
        /// </summary>
        /// <param name="context" type="AplicacaoDbContexto">Contexto da aplicacaçao</param>
        /// <param name="gestor" type="Gestor">Gestor de informação</param>
        public TecnicoController(AplicacaoDbContexto context,Gestor gestor) {
            _context = context;
            _gestor = gestor;
        }
        /// <summary>
        /// Este metodo de ação tem como objectivo de fazer o log out do Tecnico e redireciona para
        /// pagina Index do Controlador Home
        /// </summary>
        /// <returns>View Index,Controlador Home</returns>
        public async Task<IActionResult> Logout()
        {
            await _gestor.LogOut();
            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// Este metodo de ação tem como objectivo mostrar o perfil do tecnico
        /// </summary>
        /// <returns>View Perfil, Controller Tecnico</returns>
        public async Task<IActionResult> Perfil()
        {
            Utilizador user = await _gestor.getUtilizador(this.User);
            Perfil queryPerfil = _gestor.getPerfil(user);
            user.Perfil = queryPerfil;
            return View(user.Perfil);
        }
        /// <summary>
        /// Este metodo de ação mostra ao tecnico a pagina de editar perfil
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> EditarPerfil()
        {
            Utilizador user = await _gestor.getUtilizador(this.User);
            Perfil queryPerfil = _gestor.getPerfil(user);
            user.Perfil = queryPerfil;
            return View(new RegisterViewModel
            {
                Email = user.Perfil.Email,
                Telefone = user.Perfil.Telefone,
                MoradaRua = user.Perfil.MoradaRua,
                MoradaCodigoPostal = user.Perfil.MoradaCodigoPostal,
                MoradaConcelho = user.Perfil.MoradaConcelho,
                MoradaDistrito = user.Perfil.MoradaDistrito,
                Nif = user.Perfil.Nif,
                NumeroIdentificacao = user.Perfil.NumeroIdentificacao
            });
        }
        /// <summary>
        /// Este metodo de ação ocorre quando existe um post atraves do formulario da pagina EditarPefil
        /// </summary>
        /// <param name="model" type="RegisterViewModel">Parametro com informção sobre o perfil</param>
        /// <returns>Redireciona para uma ação</returns>
        [HttpPost]
        public async Task<IActionResult> EditarPerfil(RegisterViewModel model)
        {
            Utilizador user = await _gestor.getUtilizador(this.User);
            Perfil queryPerfil = _gestor.getPerfil(user);
            if (await _gestor.EditarPerfilUtilizador(model, user.Email))
            {
                return RedirectToAction("Index", "Tecnico");
            }
            else
            {
                //falta erros?
                return RedirectToAction("Index", "Tecnico");
            }
        }

        /// <summary>
        /// Este metodo de ação tem como objectivo mostrar ao tecnico uma lista de candidaturas que existem na BD
        /// ,a lista de candidaturas somente contem candidaturas com Estado igual a Estado.EM_ANALISE
        /// </summary>
        /// <returns>View ListaCandidaturas,Controller Tecnico</returns>
        public IActionResult ListaCandidaturas() {
                List<Candidatura> candidaturas = _context.Candidaturas.Where(row => row.Estado == Estado.EM_ANALISE).ToList();
            for(int i = 0; i < candidaturas.Count(); i++)
            {
                candidaturas[i].Candidato = _context.Users.Where(row => row.Id == candidaturas[i].UtilizadorFK).Single();
                candidaturas[i].Candidato.Perfil = _context.Perfils.Where(row => row.UtilizadorFK == candidaturas[i].UtilizadorFK).Single();
                candidaturas[i].TipoCandidatura = _context.TipoCandidatuas.Single(row => row.ID == candidaturas[i].TipoCandidaturaFK);
            }
            return View(candidaturas);
        }

        /// <summary>
        /// Este metodo de ação ocorre quando é efectuado um post atraves do formulario da ListaCandidaturas
        /// e tem como objectivo mostrar uma candidatura para ser feita uma analise
        /// </summary>
        /// <returns>View AnalisarCandidatura,Controller Tecnico</returns>
        [HttpPost]
        public IActionResult AnalisarCandidatura() {
           
            List<string> x = Request.Form["lista"].ToList();
            if (x.Count <= 0)
                return RedirectToAction("Index", "Tecnico"); // caso não selecione nada
            List<int> aux = new List<int>();
            for (int i = 0; i < x.Count; i++)
            {
                int a = 0;
                Int32.TryParse(x[i], out a);
                if (a != 0)
                {
                    aux.Add(a);
                    i = x.Count;
                }
            }
            Candidatura listaIns = _context.Candidaturas.Where(row => aux.Contains(row.ID)).Single();
            Utilizador user = _context.Users.Where(row => row.Id == listaIns.UtilizadorFK).Single();
            Perfil perfil = _context.Perfils.Where(row => row.ID == user.PerfilFK).Single();
            listaIns.Candidato = user;
            listaIns.Candidato.Perfil = perfil;
            return View("AnalisarCandidatura",listaIns);
        }

        /// <summary>
        ///Este metodo de ação ocorre quando é enviada informação(post) atraves do formulario AnalisarCandidatura
        ///e tem como objectivo mudar o estado de uma candidatura para Rejeitada
        /// </summary>
        /// <param name="model" type="CandidaturaViewModel">modelo de view de uma candidatura</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RejeitarCandidatura(CandidaturaViewModel model) {
            if (ModelState.IsValid)
            {
                Candidatura candidatura = _context.Candidaturas.Where(x => x.ID == model.ID).Single();
                Task<Utilizador> tecnico = _gestor.getUtilizador(this.User);
                if (_gestor.RejeitarCandidatura(candidatura, tecnico.Result))
                    return View();// rejeitada
                return View(); // erro a rejeitar
            }
            return View(); // modelstate invalid
        }

        /// <summary>
        ///Este metodo de ação ocorre quando é enviada informação(post) atraves do formulario AnalisarCandidatura
        ///e tem como objectivo mudar o estado de uma candidatura para Aprovada
        /// </summary>
        /// <param name="model" type="CandidaturaViewModel">model de view de uma candidatura</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AprovarCandidatura(CandidaturaViewModel model)
        {
            if (ModelState.IsValid)
            {
                Candidatura candidatura = _context.Candidaturas.Where(x => x.ID == model.ID).Single();
                Task<Utilizador> tecnico = _gestor.getUtilizador(this.User);
                if (_gestor.AprovarCandidatura(candidatura, tecnico.Result))
                    return View();// aprovado
                return View(); // erro a aprovar
            }
            return View(); // modelstate invalid

        }
        /// <summary>
        ///Este metodo de ação ocorre quando é enviada informação(post) atraves do formulario AnalisarCandidatura
        ///e tem como objectivo mudar o estado de uma candidatura para Incompleta
        /// </summary>
        /// <param name="model" type ="CandidaturaViewModel">modelo da view de uma candidatura</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult PedirAlteracaoCandidatura(CandidaturaViewModel model)
        {
            StringValues valores = Request.Form["lista"];
            if (!valores.Any())
                return View("Index"); // nao selecionaram valores nenhuns
            if (ModelState.IsValid)
            {
                Candidatura candidatura = _context.Candidaturas.Where(x => x.ID == model.ID)
                    .Include(row => row.Candidato)
                    .Single();
                Task<Utilizador> tecnico = _gestor.getUtilizador(this.User);
                if (_gestor.PedirAlteracaoCandidatura(candidatura, tecnico.Result,valores))
                    return View("Index");// pedido alteracao
                return View("Index"); // erro a alterar
            }
            return View("Index"); // modelstate invalid

        }
        /// <summary>
        /// Este metodo de ação  tem como objectivo mostrar uma vista com os candidatos aprovados e a possibilidade
        /// de marcar uma entrevista com os mesmos
        /// </summary>
        /// <returns></returns>
        public IActionResult MarcarEntrevista() {
            return View();
        }

        /// <summary>
        /// Este metodo de ação ocorre quando é enviada informação(post) através do formulario
        /// que está na view MarcarEntrevista
        /// </summary>
        /// <param name="model" type="MarcarEntrevistaViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult MarcarEntrevista(MarcarEntrevistaViewModel model) {
            Utilizador tecnico = _gestor.getUtilizador(this.User).Result;
            Utilizador candidato = _gestor.getUtilizadorById(model.CandidatoID);
            bool marcou = _gestor.MarcarEntrevista(tecnico, candidato, model.DataEntrevistaInicio, model.DataEntrevistaFim);
            if (marcou) {
                return View(); // entrevista marcada
            }
            return View();//erro na marcacao

        }
        /// <summary>
        /// Este metodo de ação mostra uma view com informação sobre os programas de mobilidade
        /// </summary>
        /// <returns></returns>
        public IActionResult ProgramasMobilidade()
        {
            Utilizador user = _gestor.getUtilizador(this.User).Result;
            Perfil p1 = _gestor.getPerfil(user);
            return View(p1);
        }
        /// <summary>
        /// Este metodo de ação mostra uma view com informação sobre as bolsas 
        /// </summary>
        /// <returns></returns>
        public IActionResult Bolsas()
        {
            Utilizador user = _gestor.getUtilizador(this.User).Result;
            Perfil p1 = _gestor.getPerfil(user);
            return View(p1);
        }
        /// <summary>
        /// Este metodo de ação retorna uma view com informação sobre o grupo 2
        /// </summary>
        /// <returns></returns>
        public IActionResult SobreNos()
        {
            Utilizador user = _gestor.getUtilizador(this.User).Result;
            Perfil p1 = _gestor.getPerfil(user);
            return View(p1);
        }
        /// <summary>
        /// Este metodo de ação é a ação que ocorre quando o Tecnico efectua o log in
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            Utilizador user = _gestor.getUtilizador(this.User).Result;
            Perfil p1 = _gestor.getPerfil(user);
            return View(p1);
        }
    }
}