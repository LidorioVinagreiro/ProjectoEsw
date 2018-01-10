using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProjectoEsw.Models.Candidatura_sprint2;
using ProjectoEsw.Models;

namespace ProjectoEsw.Controllers
{
    [Authorize(Roles = "Tecnico")]
    public class TecnicoController : Controller
    {
        private AplicacaoDbContexto _context;
        public TecnicoController(AplicacaoDbContexto context) {
            _context = context;
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
    }
}