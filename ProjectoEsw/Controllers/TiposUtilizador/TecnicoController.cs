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
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ListaCandidaturas() {
            List<Candidatura> candidaturas = _context.Candidaturas.Where(row => row.Estado == Estado.EM_ANALISE).ToList();
            return View("ListaCandidaturas", candidaturas);
        }

        public IActionResult AnalisarCandidatura(Candidatura model) {
            Candidatura candidatura = _context.Candidaturas.Where(row => row.ID == model.ID).Single();
            if (candidatura.Estado.Equals(Estado.EM_ANALISE))
            {
                return View("AnalisarCandidatura", model);
            }
            //erro
            return View();
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
            if (ModelState.IsValid)
            {
                Candidatura candidatura = _context.Candidaturas.Where(x => x.ID == model.ID).Single();
                Task<Utilizador> tecnico = _gestor.getUtilizador(this.User);
                if (_gestor.PedirAlteracaoCandidatura(candidatura, tecnico.Result))
                    return View();// pedido alteracao
                return View(); // erro a alterar
            }
            return View(); // modelstate invalid

        }
    }

    }