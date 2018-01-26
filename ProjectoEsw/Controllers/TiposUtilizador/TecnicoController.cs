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
    [Authorize(Roles = "Tecnico")]
    public class TecnicoController : Controller
    {
        private AplicacaoDbContexto _context;
        private Gestor _gestor;

        public TecnicoController(AplicacaoDbContexto context,Gestor gestor) {
            _context = context;
            _gestor = gestor;
        }
       
        public async Task<IActionResult> Logout()
        {
            await _gestor.LogOut();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Perfil()
        {
            Utilizador user = await _gestor.getUtilizador(this.User);
            Perfil queryPerfil = _gestor.getPerfil(user);
            user.Perfil = queryPerfil;
            return View(user.Perfil);
        }

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
        [HttpPost]
        public IActionResult AnalisarCandidatura() {
            //Candidatura candidatura = _context.Candidaturas.Where(row => row.ID == model.ID).Single();


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

        public IActionResult MarcarEntrevista() {
            return View();
        }

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

        public IActionResult ProgramasMobilidade()
        {
            Utilizador user = _gestor.getUtilizador(this.User).Result;
            Perfil p1 = _gestor.getPerfil(user);
            return View(p1);
        }

        public IActionResult Bolsas()
        {
            Utilizador user = _gestor.getUtilizador(this.User).Result;
            Perfil p1 = _gestor.getPerfil(user);
            return View(p1);
        }
        public IActionResult SobreNos()
        {
            Utilizador user = _gestor.getUtilizador(this.User).Result;
            Perfil p1 = _gestor.getPerfil(user);
            return View(p1);
        }
        public IActionResult Index()
        {
            Utilizador user = _gestor.getUtilizador(this.User).Result;
            Perfil p1 = _gestor.getPerfil(user);
            return View(p1);
        }
    }
}