using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ProjectoEsw.Models.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ProjectoEsw.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        //o dependidy injection vai tratar destas variaveis, não é necessario criar estes objectos

        public AccountController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager)
        {
            this._userManager = _userManager;
            this._signInManager = _signInManager;
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
                ApplicationUser user = new ApplicationUser { UserName = model.Email };
                //identityResult guarda informacao se a criacao do user ficou guardada e o createAsync cria um utilizador
                IdentityResult result = await _userManager.CreateAsync(user,model.Password);
                if (result.Succeeded)
                {
                    //isto faz signin ao utilizador e o segundo parametro é se o login é persistente..isto é cookies?
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                //tem que se melhorar isto caso aconteça alguns erros
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View();
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
                //1 false = persistent, 2 false = lockdownonfailure
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password,false,false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "UtilizadorRegistado",model.Email);
                }
                else {
                    ModelState.AddModelError(string.Empty, "invalid login user");
                }
            }
            return View();
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}