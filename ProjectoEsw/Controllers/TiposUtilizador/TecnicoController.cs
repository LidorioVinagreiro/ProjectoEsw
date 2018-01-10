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
        public async Task<IActionResult> AnaliseCandidatura(Candidatura model) {
            Utilizador user = await _gestor.getUtilizador(this.User);
            //falta
            return View();
        }
    }
}