using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProjectoEsw.Models.Estatisticas_sprint3.ViewModel;
using ProjectoEsw.GestorAplicacao;
using ProjectoEsw.Models.Estatisticas_sprint3;

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
            List<InstituicaoEstatisticas> ins = model.InstituicaoEstatistica;
            List<DataPoint> dataPoints = new List<DataPoint>();
                for (int i = 0; i < ins.Count; i++)
                {
                    dataPoints.Add(new DataPoint(ins[i].Instituicao.NomeInstituicao, ins[i].QuantidadeAlunosInscritos));
                }
                
            
            return View(ins);
        }
  
    }
}