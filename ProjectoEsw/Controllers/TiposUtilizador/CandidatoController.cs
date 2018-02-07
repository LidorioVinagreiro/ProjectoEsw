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
using Microsoft.AspNetCore.Http;
using ProjectoEsw.Models.Estatisticas_sprint3.ViewModel;
using ProjectoEsw.Models.Estatisticas_sprint3;
using Newtonsoft.Json;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectoEsw.Controllers
{
    /// <summary>
    /// Este controlador é só acedido por utilizadores com o Role Candidato 
    /// </summary>
    [Authorize(Roles = "Candidato")]
    public class CandidatoController : Controller
    {
        /// <summary>
        /// Atributos privados do controlador Candidato
        /// </summary>
        private Gestor _gestor;
        private AplicacaoDbContexto _contexto;
        private GestorEmail _gestorEmail;
        /// <summary>
        /// Metodo construtor do controlador Candidato
        /// </summary>
        /// <param name="gestor" type="gestor">Classe Gestora de </param>
        /// <param name="contexto" type="AplicacaoDbContexto"></param>
        /// <param name="gestorEmail" type="GestorEmail"></param>
        public CandidatoController(Gestor gestor, AplicacaoDbContexto contexto, GestorEmail gestorEmail)
        {
            _gestor = gestor;
            _contexto = contexto;
            _gestorEmail = gestorEmail;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Utilizador user = await _gestor.getUtilizador(this.User);
            Perfil queryPerfil = _gestor.getPerfil(user);
            user.Perfil = queryPerfil;
            var model = user.Perfil;
            return View(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Perfil()
        {
            Utilizador user = await _gestor.getUtilizador(this.User);
            Perfil queryPerfil = _gestor.getPerfil(user);
            user.Perfil = queryPerfil;
            return View(user.Perfil);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> EditarPerfil(RegisterViewModel model)
        {
            Utilizador user = await _gestor.getUtilizador(this.User);
            Perfil queryPerfil = _gestor.getPerfil(user);

            if (_gestor.EditarPerfilUtilizador(model, user.Email).Result)
            {
                bool aux = await _gestor.UploadFotoUtilizador(user, Request.Form.Files.Single());
                return RedirectToAction("Index", "Candidato"); // mudar para o perfil secalhar
            }
            else
            {
                //falta erros?
                return View("Erros/ErroEditarPerfil", queryPerfil);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AlterarPassword(RegisterViewModel model)
        {
            Utilizador user = await _gestor.getUtilizador(this.User);
            Perfil queryPerfil = _gestor.getPerfil(user);

            if (ModelState.IsValid)
            {

                await _gestor.EditarPassword(model);
                return RedirectToAction("Index", "Candidato");
            }
            else
            {
                //password nao foi alterada
                return View("ErroAlterarPassword", queryPerfil);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await _gestor.LogOut();
            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CandidaturaErasmus()
        {
            Utilizador user = _gestor.getUtilizador(this.User).Result;
            Perfil perfil = _gestor.getPerfil(user);
            user.Perfil = perfil;
            user.PerfilFK = perfil.ID;
            try
            {
                Candidatura aux = _contexto.Candidaturas.Where(row => row.UtilizadorFK == user.Id).Single();
                if (aux.UtilizadorFK == user.Id)
                {
                    return View("ErroCandidaturaRepetir",perfil);//se tiver uma rejeitada ou uma feita o ano passado
                }
            }
            catch
            {
            }
            CandidaturaViewModel model = new CandidaturaViewModel { UtilizadorFK = user.Id, Candidato = user, Instituicoes = _contexto.Instituicoes.Where(row => row.Interno == false).ToList() };
            return View("candidaturaErasmus", model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CandidaturaSantander()
        {
            Utilizador user = _gestor.getUtilizador(this.User).Result;
            Perfil perfil = _gestor.getPerfil(user);
            user.Perfil = perfil;
            user.PerfilFK = perfil.ID;
            try
            {
                Candidatura aux = _contexto.Candidaturas.Where(row => row.UtilizadorFK == user.Id).Single();
                if (aux.UtilizadorFK == user.Id)
                {
                    return View("ErroCandidaturaRepetir", perfil);//se tiver uma rejeitada ou uma feita o ano passado
                }
            }
            catch
            {
            }
            CandidaturaViewModel model = new CandidaturaViewModel { UtilizadorFK = user.Id, Candidato = user, Instituicoes = _contexto.Instituicoes.Where(row => row.Interno == true).ToList() };
            return View("CandidaturaSantander", model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CandidaturaErasmus(CandidaturaViewModel model)
        {
            if (ModelState.IsValid)
            {
                TipoCandidatura tipo = _contexto.TipoCandidatuas.Single(row => row.Tipo == "Erasmus");
                Utilizador user = await _gestor.getUtilizador(this.User);
                Perfil perfil = _contexto.Perfils.Single(row => row.ID == user.PerfilFK);
                List<string> x = Request.Form["lista"].ToList();
                if (x.Count <= 0)
                    return View("ErroSelecionarInstituicoes", perfil); // caso não selecione nada
                List<int> aux = new List<int>();
                for (int i = 0; i < x.Count; i++)
                {
                    int a = 0;
                    Int32.TryParse(x[i], out a);
                    if (a != 0)
                        aux.Add(a);
                }
                List<Instituicao> listaIns = _contexto.Instituicoes.Where(row => aux.Contains(row.ID)).ToList();
                if (listaIns.Where(row => row.Interno != false).Any())
                    return View("ErroCandidatura", perfil);
                    
                model.Instituicoes = listaIns;
                bool dataInicio = System.DateTime.Now.CompareTo(tipo.DataInicio) >= 0;
                bool dataFim = System.DateTime.Now.CompareTo(tipo.DataFim) < 0;
                if (!(dataInicio && dataFim))
                {
                    //não cumpre os prazos
                    return View("ErroPrazos", perfil);
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
                    CursoFrequentado = model.CursoFrequentado,
                    
                };

                bool done = await _gestor.adicionarCandidatura(user, candidatura, model);
                if (done)
                {
                    _gestorEmail.EnviarEmail(user, "Efectuou a candidatura", candidatura.ToString());
                    //sucesso
                    return View("CandidaturaSucesso", perfil);
                }
                //erro adicionar candidatura
                return View("ErroCandidatura", perfil);
            }
            //Erro model
            return View("Erro");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CandidaturaSantander(CandidaturaViewModel model)
        {
            if (ModelState.IsValid)
            {
                TipoCandidatura tipo = _contexto.TipoCandidatuas.Single(row => row.Tipo == "Santander");
                Utilizador user = await _gestor.getUtilizador(this.User);
                Perfil perfil = _contexto.Perfils.Single(row => row.ID == user.PerfilFK);
                List<string> x = Request.Form["lista"].ToList();
                if (x.Count <= 0)
                    return View("ErroSelecionarInstituicoes", perfil); // caso não selecione nada
                List<int> aux = new List<int>();
                for (int i = 0; i < x.Count; i++)
                {
                    int a = 0;
                    Int32.TryParse(x[i], out a);
                    if (a != 0)
                        aux.Add(a);
                }
                List<Instituicao> listaIns = _contexto.Instituicoes.Where(row => aux.Contains(row.ID)).ToList();
                if (listaIns.Where(row => row.Interno != true).Any())
                    return View("ErroCandidatura", perfil); // form as been tempered isto é os forms foram mudados manualmente

                model.Instituicoes = listaIns;
                bool dataInicio = System.DateTime.Now.CompareTo(tipo.DataInicio) >= 0;
                bool dataFim = System.DateTime.Now.CompareTo(tipo.DataFim) < 0;
                if (!(dataInicio && dataFim))
                {
                    //não cumpre os prazos
                    return View("ErroPrazos", perfil);
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

                bool done = await _gestor.adicionarCandidatura(user, candidatura, model);
                if (done)
                {
                    _gestorEmail.EnviarEmail(user, "Efectuou a candidatura", candidatura.ToString());
                    //sucesso
                    return View("CandidaturaSucesso", perfil);
                }
                //erro
                return View("ErroCandidatura", perfil);
            }
            //Erro
            return View("Erro");

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult VisualizarCandidatura()
        {
            Utilizador user = _gestor.getUtilizador(this.User).Result;
            Perfil p1 = _gestor.getPerfil(user);
            user.Perfil = p1;
            user.PerfilFK = p1.ID;
            try
            {
                Candidatura model = _contexto.Candidaturas.Where(row => row.Candidato.Id == user.Id)
                    .Include(x => x.Candidato)
                    .Single();
                model.Instituicoes = _contexto.Instituicoes_Candidatura.Where(row => row.CandidaturaId == model.ID)
                    .Include(x => x.Instituicao).ToList();
                return View("VisualizarCandidatura", model);
            }
            catch
            {
                return View("VisualizarCandidatura");
            }
            //model.Candidato = user;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult AlterarCandidatura()
        {
            Utilizador user = _gestor.getUtilizador(this.User).Result;
            Perfil p1 = _gestor.getPerfil(user);
            Candidatura candidatura = _contexto.Candidaturas.Where(x => x.Candidato.Id == user.Id)
                .Single();
            TipoCandidatura tipo = _contexto.TipoCandidatuas.Where(row => row.ID == candidatura.TipoCandidaturaFK).Single();
            p1.Utilizador = user;
            p1.UtilizadorFK = user.Id;
            user.Perfil = p1;
            candidatura.Candidato = user;
            candidatura.TipoCandidatura = tipo;
            candidatura.Instituicoes = _contexto.Instituicoes_Candidatura.Where(row => row.CandidaturaId == candidatura.ID).ToList();
            if (tipo.Tipo.Equals("Erasmus"))
                return View("AlterarCandidaturaErasmus",candidatura);
            else
                return View("AlterarCandidaturaSantander", candidatura);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AlterarCandidatura(CandidaturaViewModel model) {
            if (ModelState.IsValid) {
                Utilizador user = _gestor.getUtilizador(this.User).Result;
                Candidatura candidatura = _contexto.Candidaturas.Where(row => row.UtilizadorFK == user.Id).Single();
                candidatura.AfiliacaoEmergencia = model.AfiliacaoEmergencia;
                candidatura.AnoCurricular = model.AnoCurricular;
                candidatura.Bolsa = model.Bolsa;
                candidatura.Escola = model.Escola;
                candidatura.Estado = Estado.EM_ANALISE;
                candidatura.IBAN = model.IBAN;
                candidatura.NomeEmergencia = model.NomeEmergencia;
                candidatura.NumeroEmergencia = model.NumeroEmergencia;
                candidatura.CursoFrequentado = model.CursoFrequentado;
                _contexto.SaveChanges();
                List<string> x = Request.Form["lista"].ToList();
                if (x.Count <= 0)
                    return RedirectToAction("Index", "Candidato"); // caso não selecione nada
                List<int> aux = new List<int>();
                for (int i = 0; i < x.Count; i++)
                {
                    int a = 0;
                    Int32.TryParse(x[i], out a);
                    if (a != 0)
                        aux.Add(a);
                }
                List<Instituicao> listaIns = _contexto.Instituicoes.Where(row => aux.Contains(row.ID)).ToList();
                
                model.Instituicoes = listaIns;

                if (_gestor.AlterarCandidatura(user, candidatura, model).Result)
                    return View("Index","Candidato"); // correu tudo bem
                return View(); //Erro
            }

            return View();// algo errado no model state
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult ProgramasMobilidade()
        {
            Utilizador user = _gestor.getUtilizador(this.User).Result;
            Perfil p1 = _gestor.getPerfil(user);
            return View(p1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Bolsas()
        {
            Utilizador user = _gestor.getUtilizador(this.User).Result;
            Perfil p1 = _gestor.getPerfil(user);
            return View(p1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult SobreNos()
        {
            Utilizador user = _gestor.getUtilizador(this.User).Result;
            Perfil p1 = _gestor.getPerfil(user);
            return View(p1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult MarcarReuniao()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult MarcarReuniao(MarcarReuniaoViewModel viewModel)
        {
            Utilizador actual = _gestor.getUtilizador(this.User).Result;
            Perfil perfil = _gestor.getPerfil(actual);
            bool marcou = _gestor.MarcarReuniao(actual, viewModel.DataReuniaoInicio, viewModel.DataReuniaoFim);
            if (marcou)
                return View("MarcarReuniaoSucesso", perfil); // marcou reuniao
            return View("ErroMarcarReuniao", perfil);//erro nao marcou    
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idPerfil"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetEvents(int idPerfil)
        {
            var events = _contexto.Eventos.Where(Eventos => Eventos.PerfilFK == idPerfil).ToList();
            return Json(events);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="evento"></param>
        /// <returns></returns>
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
            else
            {
                _contexto.Eventos.Add(evento);
            }

            await _contexto.SaveChangesAsync();
            status = true;
            return new JsonResult(new { Data = new { status = status } });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> DeleteEvent(int id)
        {
            var status = false;
            Eventos evento = _contexto.Eventos.Where(even => even.ID == id).SingleOrDefault();

            if (evento != null)
            {
                _contexto.Eventos.Remove(evento);
                status = true;
            }
            await _contexto.SaveChangesAsync();
            return new JsonResult(new { status = status });
        }

    }
}
