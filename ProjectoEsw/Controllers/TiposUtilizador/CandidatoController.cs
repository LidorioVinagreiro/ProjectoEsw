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
        private AplicacaoDbContexto _contexto;

        public CandidatoController(Gestor gestor,AplicacaoDbContexto contexto) {
            _gestor = gestor;
            _contexto = contexto;
        }

        // GET: /<controller>/
        [HttpGet]
        public async Task<IActionResult> Index([FromServices] AplicacaoDbContexto context)
        {
           Utilizador user = await _gestor.getUtilizador(this.User);
            Perfil queryPerfil = (from perfil in context.Perfils
                               where perfil.ID == user.PerfilFK
                               select new Perfil
                               {
                                   NomeCompleto = perfil.NomeCompleto,
                                   Email = perfil.Email,
                                 
                                   NumeroIdentificacao = perfil.NumeroIdentificacao,
                                   DataNasc = perfil.DataNasc,
                                   Nif = perfil.Nif,
                                   Telefone = perfil.Telefone
                                   
                               }).FirstOrDefault();
            user.Perfil = queryPerfil;
           return View(user.Perfil);
        }




        public IActionResult Perfil() {
            return View("EditarPerfil");
        }

        [HttpPost]
        public async Task<IActionResult> EditarPerfil(RegisterViewModel model) {
            if (await _gestor.EditarPerfilUtilizador(model))
            {
                return RedirectToAction("Index", "Candidato");
            }
            else {
                //falta erros?
                return RedirectToAction("Index", "Candidato");
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> AlterarPassword(RegisterViewModel model) {
            if (ModelState.IsValid)
            {

                await _gestor.EditarPassword(model);
                return RedirectToAction("Index","Candidato");
            }
            else {
                //password nao foi alterada
                return RedirectToAction("Index", "Candidato");
            }
        }

        public async Task<IActionResult> Logout() {
            await _gestor.LogOut();
            return RedirectToAction("Index", "Home");
        }


        //METODOS DE AJAX
        [HttpGet]
        public async Task<JsonResult> GetEvents()
        {
            Utilizador user = await _gestor.getUtilizador(this.User);
            var events = _contexto.Eventos.Where(Eventos => Eventos.Utilizador.ID == user.PerfilFK).ToList();
            return Json(events);
        }
    }
}
