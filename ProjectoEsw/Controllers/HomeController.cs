﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectoEsw.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using ProjectoEsw.GestorAplicacao;
using ProjectoEsw.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using ProjectoEsw.Models;

namespace ProjectoEsw.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private Gestor _gestor;
        //o dependidy injection vai tratar destas variaveis, não é necessario criar estes objectos
        private SignInManager<Utilizador> _signInManager;
        private UserManager<Utilizador> _userManager;
        private AplicacaoDbContexto _context;

        public HomeController(
            SignInManager<Utilizador> signInManager,
            UserManager<Utilizador> userManager,
            AplicacaoDbContexto contexto,
            Gestor gestor)
        {
            _context = contexto;
            _signInManager = signInManager;
            _userManager = userManager;
            _gestor = gestor;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        //este metodo é o que corre apos o utilizador submeter informacao e o 
        //RegisterViewModel é o modelo da informacao que é envida pela View do Register

        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) {
                Utilizador user = new Utilizador { UserName = model.Email ,Email = model.Email};
                IdentityResult resultCreate = await _userManager.CreateAsync(user, model.Password);

                if (resultCreate.Succeeded) {
                    await _context.SaveChangesAsync();
                    user.PerfilFK = await _gestor.criarPerfilUtilizador(model, user.Id);
                    IdentityResult resultadoRole = await _userManager.AddToRoleAsync(user, "Candidato");

                    if (resultadoRole.Succeeded)
                    {
                        await _context.SaveChangesAsync();
                        string ctoken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        string tokenlink = Url.Action("ConfirmarEmail", "Home", new
                        {
                            userId = user.Id,
                            token = ctoken
                        }, protocol: HttpContext.Request.Scheme);

                        await _context.SaveChangesAsync();
                        GestorEmail gm = new GestorEmail();
                        gm.EnviarEmail(user, "Novo Utilizador", tokenlink);

                        return View("ConfirmarEmail");
                    }
                    else {
                        //erro de adicionar role
                        ModelState.AddModelError(string.Empty, "OCURREU UM ERRO ROLE");
                        return View("Error");
                    }
                }
                else {
                    ModelState.AddModelError(string.Empty, "OCURREU UM ERRO CRIAR");
                    //erro de criar utilizador
                    return View("Error");
                }
        }
        else {
            //modelState not valid
            ModelState.AddModelError(string.Empty, "OCURREU UM ERRO");
            return View();
        }
                
    }

        public async Task<IActionResult> ConfirmarEmail(string userId, string token) {
            if (userId == null || token == null)
            {
                //userid ou token null
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) {
                //nao encontra utilizador
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                //adicionar que a confirmacao do emai foi efectuada
                return RedirectToAction("Login", "Home");
            }
            else {
                //nao houve confirmacao do token
                return View("Error");
            }

        }
       
        public IActionResult Login() {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model) {
            if (ModelState.IsValid)
            {
                ////1 false = persistent, 2 false = lockdownonfailure
                Utilizador user = await _userManager.FindByNameAsync(model.Email);
                if (user.EmailConfirmed)
                {

                    Microsoft.AspNetCore.Identity.SignInResult resultado = await _signInManager.PasswordSignInAsync(
                        model.Email, model.Password, false, false
                    );
                    if (resultado.Succeeded)
                    {
                        string role = await _gestor.getUtilizadorRole(model.Email);
                        switch (role)
                        {

                            case "Candidato":
                                return RedirectToAction("Index", "Candidato");

                            case "administrador":
                                return RedirectToAction("Index", "Administrador");

                            case "Tecnico":
                                return RedirectToAction("Index", "Tecnico");

                            default:
                                ModelState.AddModelError(string.Empty, "Erro no role Login");
                                return RedirectToAction("Login", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "invalid login");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "falta Confirmar Email");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "model errado");
                return View();
            }
        }

        public IActionResult RecuperarPassword() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecuperarPassword([Bind] string Email)
        {
            Utilizador user = await _userManager.FindByNameAsync(Email);
            if(user == null || !(await _userManager.IsEmailConfirmedAsync(user))){
                //nao ha utilizador ou falta confirmar email.. nao defenir erro
                View();
            }


            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callback = Url.Action(
                "ResetPassword", 
                "Home", 
                new { Email = user.UserName , Code = token}, 
                protocol: HttpContext.Request.Scheme
                );
            GestorEmail gm = new GestorEmail();
            gm.EnviarEmail(user, "reset password", callback.ToString());
            //falta informacao que pass ja foi enviada
            return View();
        }

        public IActionResult ResetPassword(string code) {
            ViewBag.code = code;
            return code == null ? View("Error") : View() ;
        }
            
        [HttpPost]
        public async Task<IActionResult> ResetPassword(RecuperarPassViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Error");

            Utilizador user = await _userManager.FindByNameAsync(model.Email); 
            if (user == null || model.Code == null)
                return View("RecuperarPassword");

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            
            if (result.Succeeded) {
                ModelState.AddModelError(string.Empty, "Password Alterada Faça Login");
                return View("Login");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Password Alterada Faça Login");
                return View("Error");
            }
        
        }
      
        public async Task <IActionResult> Index()
        {
             await _gestor.adicionarInfo();
            return View();
        }
        
    }

    

}
