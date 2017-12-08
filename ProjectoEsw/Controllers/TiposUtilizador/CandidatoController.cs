using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProjectoEsw.GestorAplicacao;
using ProjectoEsw.Models.Identity;
using ProjectoEsw.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectoEsw.Controllers
{
    [Authorize(Roles ="Candidato")]
    public class CandidatoController : Controller
    {
        private Gestor _gestor;

        public CandidatoController(Gestor gestor) {
            _gestor = gestor;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index([FromServices] AplicacaoDbContexto context)
        {
           Utilizador user = await _gestor.getUtilizador(this.User);
            Perfil queryPerfil = (from perfil in context.Perfils
                               where perfil.ID == user.PerfilFK
                               select new Perfil
                               {
                                   NomeCompleto = perfil.NomeCompleto,
                                   Email = perfil.Email,
                                   Morada = perfil.Morada
                               }).FirstOrDefault();
            user.Perfil = queryPerfil;
           return View(user.Perfil);
        }

        public IActionResult Perfil() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditarPerfil() {
            await _gestor.adicionarInfo();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AlterarPassword() {
            await _gestor.adicionarInfo();
            return View();
        }

        public async Task<IActionResult> Logout() {

            return RedirectToAction("Home", "Login");
        }
    }
}
