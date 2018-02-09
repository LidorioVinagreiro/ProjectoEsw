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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjectoEsw.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [AllowAnonymous]
    public class HomeController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        private Gestor _gestor;
        private SignInManager<Utilizador> _signInManager;
        private UserManager<Utilizador> _userManager;
        private AplicacaoDbContexto _context;
        private GestorEmail _gestorEmail;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="signInManager"></param>
        /// <param name="userManager"></param>
        /// <param name="contexto"></param>
        /// <param name="gestor"></param>
        /// <param name="gestorEmail"></param>
        public HomeController(
            SignInManager<Utilizador> signInManager,
            UserManager<Utilizador> userManager,
            AplicacaoDbContexto contexto,
            Gestor gestor,
            GestorEmail gestorEmail)
        {
            _context = contexto;
            _signInManager = signInManager;
            _userManager = userManager;
            _gestor = gestor;
            _gestorEmail = gestorEmail;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        //este metodo é o que corre apos o utilizador submeter informacao e o 
        //RegisterViewModel é o modelo da informacao que é envida pela View do Register
        /// <summary>
        /// 
        /// </summary>
        /// <param name="RegisterViewModel"></param>
        /// <returns></returns>
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
                        //GestorEmail gm = new GestorEmail();
                        _gestorEmail.EnviarEmail(user, "Novo Utilizador", tokenlink);

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<IActionResult> ConfirmarEmail(string userId, string token) {
            ViewData["Title"] = "Erro Email Confirmado";
            if (userId == null || token == null)
            {
                //userid ou token null
                return View("../Erros/ErroConfirmarEmail");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) {
                //nao encontra utilizador
                return View("../Erros/ErroConfirmarEmail");
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View("EmailConfirmado");
            }
            else {
                //nao houve confirmacao do token
                return View("Erros/ErroConfirmarEmail");
            }

        }
       /// <summary>
       /// 
       /// </summary>
       /// <returns></returns>
        public IActionResult Login() {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model) {
            if (ModelState.IsValid)
            {
                Utilizador user = await _userManager.FindByNameAsync(model.Email);
                if (user != null)
                {

                    if (user.EmailConfirmed)
                    {
                        Microsoft.AspNetCore.Identity.SignInResult resultado = await _signInManager.PasswordSignInAsync( model.Email, model.Password, false, false );
                        if (resultado.Succeeded)
                        {
                            string role = await _gestor.getUtilizadorRole(model.Email);
                            switch (role)
                            {

                                case "Candidato":
                                    return RedirectToAction("Index", "Candidato");

                                case "Administrador":
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
                            ModelState.AddModelError(string.Empty, "Email e/ou password do utilizador está incorrecta");
                            return View();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Falta confirmar Email");
                        return View();
                    }
                }
                else {
                    ModelState.AddModelError(string.Empty, "Utilizador não existe");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Model errado");
                return View();
            }
        }

        public IActionResult RecuperarPassword() {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RecuperarPassword([Bind] string Email)
        {
            Utilizador user = await _userManager.FindByNameAsync(Email);
            if(user == null || !(await _userManager.IsEmailConfirmedAsync(user))){
                ModelState.AddModelError(string.Empty, "Email do utilizador não existe");
                return View();
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callback = Url.Action(
                "ResetPassword", 
                "Home", 
                new { Email = user.UserName , Code = token}, 
                protocol: HttpContext.Request.Scheme
                );
            //GestorEmail gm = new GestorEmail();
            _gestorEmail.EnviarEmail(user, "reset password", callback.ToString());
            return View("ConfirmarPassword");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public IActionResult ResetPassword(string code) {
            ViewBag.code = code;
            return code == null ? View("Error") : View() ;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
                return View("PasswordAlterada");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Password não coincide");
                return View();
            }
        } 
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult ProgramasMobilidade()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult SobreNos()
        {
            return View();
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Bolsas()
        {
            return View();
        }
    }
}

