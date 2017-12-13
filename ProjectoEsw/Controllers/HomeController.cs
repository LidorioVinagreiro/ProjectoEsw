﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectoEsw.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using ProjectoEsw.GestorAplicacao;
using ProjectoEsw.Models.ViewModels;

namespace ProjectoEsw.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private Gestor _gestor;
        //o dependidy injection vai tratar destas variaveis, não é necessario criar estes objectos

        public HomeController(Gestor _gestor)
        {
            this._gestor = _gestor;
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult Register()
        {
            return View();
        }

        //este metodo é o que corre apos o utilizador submeter informacao e o 
        //RegisterViewModel é o modelo da informacao que é envida pela View do Register

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) {
                bool result = await _gestor.criarUtilizador(model);
                return RedirectToAction("Index", "Candidato");
            }
            return View();
        }

        [AutoValidateAntiforgeryToken]
        public IActionResult Login() {
            return View();
        }

        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model) {
            if (ModelState.IsValid) {
                ////1 false = persistent, 2 false = lockdownonfailure
                Microsoft.AspNetCore.Identity.SignInResult resultado = await _gestor.autenticarUtilizador(model);

                if (resultado.Succeeded)
                {
                    switch (_gestor.getUtilizadorRole(this.User).ToString())
                    {
                        case "Candidato":
                            return RedirectToAction("Index", "Candidato");

                        case "administrador":
                            return RedirectToAction("Index", "Administrador");

                        case "Tecnico":
                            return RedirectToAction("Index", "Tecnico");

                        default:
                            return RedirectToAction("Login", "Home");
                    }
                }
                else {
                    ModelState.AddModelError(string.Empty, "invalid login");
                }
            }
            return View();
        }

        public IActionResult RecuperarPassword() {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RecuperarPassword(RecuperarPassViewModel model)
        {
            bool result = await _gestor.RecuperarPassword(model);
            if (result)
            {
                return RedirectToAction("Index", "Candidato");
            }
            else {
                return RedirectToAction("RecuperarPassword", "Home");
            }
        }

        public async Task <IActionResult> Index()
        {
             await _gestor.adicionarInfo();
            return View();
        }
        
    }

    

}

