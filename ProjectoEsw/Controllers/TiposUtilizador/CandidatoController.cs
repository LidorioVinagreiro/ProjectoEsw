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
        [HttpGet]
        public async Task<IActionResult> Index()
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
                Telefone = user.Perfil.Telefone
                
            });
        }

        public async Task<IActionResult> Perfil() {
            Utilizador user = await _gestor.getUtilizador(this.User);
            Perfil queryPerfil = _gestor.getPerfil(user);
            user.Perfil = queryPerfil;
            return View(user.Perfil);
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
    }
}
