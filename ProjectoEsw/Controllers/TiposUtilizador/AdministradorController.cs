using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ProjectoEsw.Controllers
{
    [Authorize(Roles="Administrador")]
    public class AdministradorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}