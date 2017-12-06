using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ProjectoEsw.Models.Identity;

namespace ProjectoEsw.Controllers
{
    public class UtilizadorRegistadoController : Controller
    {
        private UserManager<AplicacaoUtilizador> _userManager;

        public UtilizadorRegistadoController(UserManager<AplicacaoUtilizador> _userManager) {
            this._userManager = _userManager; 
        }

        [Authorize]
        public IActionResult Index(string email)
        {
            if (ModelState.IsValid)
            {

                //pode se usar isto pq o this.user é um claim..
                // _userManager.GetUserAsync(this.User)
                //ViewBag.Message = "Email: " + model.Email + "\n password: " + model.Password;

                return View();
            }
            else {
                return null;
            }
                
        }
        //argumento - (Perfil model)
        //returnava("index")
        //[HttpPost]
        public IActionResult EditarPerfil() {

            return View();
        }
        

        
    }
}
