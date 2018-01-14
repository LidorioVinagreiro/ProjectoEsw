using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProjectoEsw.Models.Estatisticas_sprint3.ViewModel;
using ProjectoEsw.GestorAplicacao;

namespace ProjectoEsw.Controllers
{
    [Authorize(Roles="Administrador")]
    public class AdministradorController : Controller
    {
        private Gestor _gestor;
        public AdministradorController(Gestor gestor) {
            _gestor = gestor;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult VerEstatisticas() {
            EstatisticasGerais model = _gestor.GerarEstatisticas();
            return View(model);
        }
  
    }
}