using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProjectoEsw.GestorAplicacao;
using ProjectoEsw.Models.Identity;
using ProjectoEsw.Models;
using ProjectoEsw.Models.Calendario;
using ProjectoEsw.Models.Candidatura_sprint2;
using ProjectoEsw.Models.Candidatura_sprint2.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectoEsw.Controllers
{
    [Authorize(Roles ="Candidato")]
    public class CandidatoController : Controller
    {
        private Gestor _gestor;
        private AplicacaoDbContexto _contexto;
        private GestorEmail _gestorEmail;
        public CandidatoController(Gestor gestor,AplicacaoDbContexto contexto,GestorEmail gestorEmail) {
            _gestor = gestor;
            _contexto = contexto;
            _gestorEmail = gestorEmail;
        }

        // GET: /<controller>/
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Utilizador user = await _gestor.getUtilizador(this.User);
            Perfil queryPerfil = _gestor.getPerfil(user);
            user.Perfil = queryPerfil;
            var model = user.Perfil;
           return View(model);
        }


        public async Task<IActionResult> EditarPerfil()
        {
            Utilizador user = await _gestor.getUtilizador(this.User);
            Perfil queryPerfil = _gestor.getPerfil(user);
            user.Perfil = queryPerfil;
            return View(new RegisterViewModel
            {
                Email = user.Perfil.Email,
                Telefone = user.Perfil.Telefone,
                MoradaRua = user.Perfil.MoradaRua,
                MoradaCodigoPostal = user.Perfil.MoradaCodigoPostal,
                MoradaConcelho = user.Perfil.MoradaConcelho,
                MoradaDistrito = user.Perfil.MoradaDistrito,
                Nif = user.Perfil.Nif,
                NumeroIdentificacao = user.Perfil.NumeroIdentificacao
            });
        }

        public async Task<IActionResult> Perfil() {
            Utilizador user = await _gestor.getUtilizador(this.User);
            Perfil queryPerfil = _gestor.getPerfil(user);
            user.Perfil = queryPerfil;
            return View(user.Perfil);
        }

        [HttpPost]
        public async Task<IActionResult> EditarPerfil(RegisterViewModel model) {
            Utilizador user = await _gestor.getUtilizador(this.User);
            Perfil queryPerfil = _gestor.getPerfil(user);
            if (await _gestor.EditarPerfilUtilizador(model, user.Email))
            {
                return RedirectToAction("Index", "Candidato");
            }
            else {
                //falta erros?
                return RedirectToAction("Index", "Candidato");
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> AlterarPassword(RegisterViewModel model) {
            if (ModelState.IsValid)
            {

                await _gestor.EditarPassword(model);
                return RedirectToAction("Index","Candidato");
            }
            else {
                //password nao foi alterada
                return RedirectToAction("Index", "Candidato");
            }
        }

        public async Task<IActionResult> Logout() {
            await _gestor.LogOut();
            return RedirectToAction("Index", "Home");
        }


        public IActionResult CandidaturaErasmus() {
            Utilizador user = _gestor.getUtilizador(this.User).Result;
            Perfil perfil = _gestor.getPerfil(user);
            user.Perfil = perfil;
            user.PerfilFK = perfil.ID;
            CandidaturaViewModel model = new CandidaturaViewModel { UtilizadorFK = user.Id ,Candidato = user, Instituicoes = _contexto.Instituicoes.Where(row => row.Interno == false).ToList() };
            return View("candidaturaErasmus",model);
        }

        public IActionResult CandidaturaSantander()
        {
            Utilizador user = _gestor.getUtilizador(this.User).Result;
            Perfil perfil = _gestor.getPerfil(user);
            user.Perfil = perfil;
            user.PerfilFK = perfil.ID;
            CandidaturaViewModel model = new CandidaturaViewModel { UtilizadorFK = user.Id, Candidato = user, Instituicoes = _contexto.Instituicoes.Where(row => row.Interno == true).ToList() };
            return View("CandidaturaSantander",model);
        }

        [HttpPost]
        public async Task<IActionResult> CandidaturaErasmus(CandidaturaViewModel model) {           
            if (ModelState.IsValid) {
                TipoCandidatura tipo = _contexto.TipoCandidatuas.Single(row => row.Tipo == "Erasmus");
                Utilizador user = await _gestor.getUtilizador(this.User);
                StringValues x = Request.Form["lista"];
    //            List<Instituicao> insti = _contexto.Instituicoes.Where(row => x.AsParallel<IEnumerable<string>>.Contains(row.ID));
                bool dataInicio = System.DateTime.Now.CompareTo(tipo.DataInicio) >=0;
                bool dataFim = System.DateTime.Now.CompareTo(tipo.DataFim) < 0;
                if (!(dataInicio && dataFim)) {
                    //não cumpre os prazos
                    return View();
                }
                
                Candidatura candidatura = new Candidatura
                {
                    UtilizadorFK = user.Id,
                    TipoCandidaturaFK = tipo.ID,
                    Candidato = user,
                    CartaMotivacao = model.CartaMotivacao,
                    IBAN = model.IBAN,
                    AnoCurricular = model.AnoCurricular,
                    Bolsa = model.Bolsa,
                    Escola = model.Escola,
                    Estado = Estado.EM_ANALISE,
                    AfiliacaoEmergencia = model.AfiliacaoEmergencia,
                    NumeroEmergencia = model.NumeroEmergencia,
                    NomeEmergencia = model.NomeEmergencia,
                    CursoFrequentado = model.CursoFrequentado
                };
                bool done = await _gestor.adicionarCandidatura(user, candidatura,model);
                if (done) {
                    _gestorEmail.EnviarEmail(user, "Efectuou a candidatura", candidatura.ToString());
                    //sucesso
                    return View("Index");
                }
                //erro adicionar candidatura
                return View();
            }
            //Erro model
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CandidaturaSantander(CandidaturaViewModel model)
        {
            if (ModelState.IsValid)
            {
                TipoCandidatura tipo = _contexto.TipoCandidatuas.Single(row => row.Tipo == "Santander");
                Utilizador user = await _gestor.getUtilizador(this.User);
                var x = Request.Form["lista"];
                bool dataInicio = System.DateTime.Now.CompareTo(tipo.DataInicio) >= 0;
                bool dataFim = System.DateTime.Now.CompareTo(tipo.DataFim) < 0;
                if (!(dataInicio && dataFim))
                {
                    //não cumpre os prazos
                    return View();
                }
                Candidatura candidatura = new Candidatura
                {
                    UtilizadorFK = user.Id,
                    TipoCandidaturaFK = tipo.ID,
                    Candidato = user,
                    CartaMotivacao = model.CartaMotivacao,
                    IBAN = model.IBAN,
                    AnoCurricular = model.AnoCurricular,
                    Bolsa = model.Bolsa,
                    Escola = model.Escola,
                    Estado = Estado.EM_ANALISE,
                    AfiliacaoEmergencia = model.AfiliacaoEmergencia,
                    NumeroEmergencia = model.NumeroEmergencia,
                    NomeEmergencia = model.NomeEmergencia,
                };
                bool done = await _gestor.adicionarCandidatura(user, candidatura,model);
                if (done)
                {
                    _gestorEmail.EnviarEmail(user, "Efectuou a candidatura", candidatura.ToString());
                    //sucesso
                    return View("Index");
                }
                //erro
                return View();
            }
            //Erro
            return View();

        }

        public IActionResult VisualizarCandidatura() {
            Utilizador user = _gestor.getUtilizador(this.User).Result;
            Perfil p1 = _gestor.getPerfil(user);
            user.Perfil = p1;
            user.PerfilFK = p1.ID;
            Candidatura model = _contexto.Candidaturas.Where(row => row.Candidato.Id == user.Id)
                .Include(x=>x.Candidato)
                .Include(x=>x.Instituicoes)
                .Single();
            //model.Candidato = user;
            return View("VisualizarCandidatura", model);
        }
        public IActionResult AlterarCandidatura() {
            Utilizador user = _gestor.getUtilizador(this.User).Result;
            Perfil p1 = _gestor.getPerfil(user);
            Candidatura candidatura = _contexto.Candidaturas.Where(x => x.Candidato.Id == user.Id).Single();
            return View("AlterarCandidatura",candidatura);
        }

        [HttpPost]
        public IActionResult AlterarCandidatura(CandidaturaViewModel model) {
            return View();
        }

        public IActionResult ProgramasMobilidade() {
            Utilizador user = _gestor.getUtilizador(this.User).Result;
            Perfil p1 = _gestor.getPerfil(user);
            return View(p1);
        }

        public IActionResult Bolsas()
        {
            Utilizador user = _gestor.getUtilizador(this.User).Result;
            Perfil p1 = _gestor.getPerfil(user);
            return View(p1);
        }
        public IActionResult SobreNos()
        {
            Utilizador user = _gestor.getUtilizador(this.User).Result;
            Perfil p1 = _gestor.getPerfil(user);
            return View(p1);
        }
        //METODOS DE AJAX
        [HttpGet]
        public JsonResult GetEvents(int idPerfil)
        {
            var events = _contexto.Eventos.Where(Eventos => Eventos.PerfilFK == idPerfil).ToList();
            return Json(events);
        }

        [HttpPost]
        public async Task<JsonResult> SaveEvents(Eventos evento)
        {
            
            var status = false;
            if (evento.ID > 0)
            {
                Utilizador user = await _gestor.getUtilizador(this.User);
                var updateEvento = _contexto.Eventos.Where(even => even.ID == evento.ID).SingleOrDefault();
                updateEvento.Inicio = evento.Inicio;
                updateEvento.Titulo = evento.Titulo;
                updateEvento.Fim = evento.Fim;
                updateEvento.Descricao = evento.Descricao;
                updateEvento.PerfilFK = _gestor.getPerfil(user).ID;

            }
            else {
                _contexto.Eventos.Add(evento);
            }
            
            await _contexto.SaveChangesAsync();
            status = true;
            return  new JsonResult(new { Data = new { status = status } });
        }

        [HttpPost]
        public async Task<JsonResult> DeleteEvent(int id) {
            var status = false;
            Eventos evento = _contexto.Eventos.Where(even => even.ID == id).SingleOrDefault();

            if (evento!=null){
                _contexto.Eventos.Remove(evento);
                status = true;
            }
            await _contexto.SaveChangesAsync();
            return new JsonResult(new { status = status} ); 
        }
    }
}
