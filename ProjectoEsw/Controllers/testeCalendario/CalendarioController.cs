using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectoEsw.Models;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectoEsw.testeCalendario
{
    public class CalendarioController : Controller
    {
        private AplicacaoDbContexto _context;

        public CalendarioController(AplicacaoDbContexto contexto) {
            _context = contexto;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetEvents() {
            var events = _context.Eventos.ToList();
            return Json(events);
        }
    }
}
