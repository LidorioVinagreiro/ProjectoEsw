using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProjectoEsw.Models.Estatisticas_sprint3.ViewModel;
using ProjectoEsw.GestorAplicacao;
using ProjectoEsw.Models.Estatisticas_sprint3;
using Newtonsoft.Json;

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

        public async Task<IActionResult> Logout()
        {
            await _gestor.LogOut();
            return RedirectToAction("Index", "Home");
        }


        public IActionResult VerEstatisticas()
        {
            EstatisticasGerais model = _gestor.GerarEstatisticas();
            List<InstituicaoEstatisticas> ins = model.InstituicaoEstatistica;
            List<TipoQuantidadeEstatistica> can = model.TipoQuantidadeEstatisticas;
            TotalBolsaEstatisticas bol = model.TotalBolsaEstatisticas;
            List<ARCandidaturasEstatisticas> arc = model.ARCandidaturaEstatisticas;
            DestinosPreferenciasEstatisticas dpe = model.DestinosPreferenciasEstatisticas;
            // Instituicoes
            List<DataPoint> dataPointsIns = new List<DataPoint>();
            for (int i = 0; i < ins.Count; i++)
            {
                dataPointsIns.Add(new DataPoint(ins[i].Instituicao.NomeInstituicao, ins[i].QuantidadeAlunosInscritos));
            }
            ViewBag.dataPointsIns = JsonConvert.SerializeObject(dataPointsIns);
            // Tipo Candidatura
            List<DataPoint> dataPointsTipo = new List<DataPoint>();
            for (int a = 0; a < can.Count; a++)
            {
                dataPointsTipo.Add(new DataPoint(can[a].TipoMobilidade, can[a].nCandidatos));
            }
            ViewBag.dataPointsTipo = JsonConvert.SerializeObject(dataPointsTipo);
            // Bolsas
            List<DataPoint> dataPointBolsas = new List<DataPoint>();
            dataPointBolsas.Add(new DataPoint("Com Bolsa", bol.QuantidadeBolsas));
            dataPointBolsas.Add(new DataPoint("Sem Bolsa", bol.QuantidadeSemBolsa));
            ViewBag.dataPointsBolsa = JsonConvert.SerializeObject(dataPointBolsas);
            // AR candidaturas
            List<DataPoint> dataPointsArc = new List<DataPoint>();
            for (int x = 0; x < arc.Count; x++)
            {
                dataPointsArc.Add(new DataPoint(arc[x].Estado, arc[x].intNCandidaturasEstado));
            }
            ViewBag.dataPointsArc = JsonConvert.SerializeObject(dataPointsArc);
            // Destino
            ViewBag.maiorPreferencia = dpe.PreferenciaMaior.NomeInstituicao;
            ViewBag.menorPreferencia = dpe.PreferenciaMenor.NomeInstituicao;

            return View();
        }
    }
}
