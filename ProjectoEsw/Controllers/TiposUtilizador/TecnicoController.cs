using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ProjectoEsw.Controllers
{
    public class TecnicoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}