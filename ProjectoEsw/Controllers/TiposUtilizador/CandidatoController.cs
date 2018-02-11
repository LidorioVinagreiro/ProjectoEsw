﻿using System;
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
        /// Esta pagina é apresentada quando o utilizador do tipo Candidato efectua o login 
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
        /// Esta ação possibilita a alteração do perfil do utilizador
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
        /// Esta ação possibilita a visualização do utilizador
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
        /// Esta ação ocorre após ser submetida informação atraves de um formulario.
        /// O formulario encontra-se na view EditarPerfil
        /// </summary>
        /// <param name="model">Modelo que contem dados a serem alterados</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> EditarPerfil(RegisterViewModel model)
        {
            Utilizador user = await _gestor.getUtilizador(this.User);
            Perfil queryPerfil = _gestor.getPerfil(user);

            if (_gestor.EditarPerfilUtilizador(model, user.Email).Result)
            {
                bool aux = await _gestor.UploadFotoUtilizador(user, Request.Form.Files.Single());
                return View("EditarPerfilSucesso",queryPerfil); // mudar para o perfil secalhar
            }
            else
            {
                //falta erros?
                return View("../Erros/ErroEditarPerfil", queryPerfil);
            }

        }

        /// <summary>
        /// Esta ação ocorre apos a submissão de dados atraves do formulario da view EditarPerfil
        /// </summary>
        /// <param name="model"> Modelo com dados inseridos pelo utilizador</param>
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
                return View("../Erros/ErroAlterarPassword", queryPerfil);
            }
        }
        /// <summary>
        /// Esta ação efectua o log out do utilizador actual
        /// </summary>
        /// <returns>Redireciona para a pagina inicial (Home/Index)</returns>
        public async Task<IActionResult> Logout()
        {
            await _gestor.LogOut();
            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// Esta ação é responsavel pela construção da view candidaturaErasmus.
        /// E possibilita ao utilizador efectuar uma candidatura do tipo Erasmus
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
                    return View("ErroCandidaturaRepetir",perfil);
                }
            }
            catch
            {
            }
            CandidaturaViewModel model = new CandidaturaViewModel { UtilizadorFK = user.Id, Candidato = user, Instituicoes = _contexto.Instituicoes.Where(row => row.Interno == false).ToList() };
            return View("candidaturaErasmus", model);
        }
        /// <summary>
        /// Esta ação é responsavel pela construção da view candidaturaSantander.
        /// E possibilita ao utilizador efectuar uma candidatura do tipo Santander       
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
        /// Esta ação ocorre quando o utilizador preenche e envia o formulario existente na view candidaturaErasmus
        /// Esta ação pretende verificação dos dados submetidos e guardar esta mesma informação
        /// </summary>
        /// <param name="model">modelo de dados de uma candidatura</param>
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
                    return View("../Erros/ErroSelecionarInstituicoes", perfil); // caso não selecione nada
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
                    return View("../Erros/ErroCandidatura", perfil);
                    
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
                return View("../Erros/ErroCandidatura", perfil);
            }
            //Erro model
            return View("Erro");
        }
        /// <summary>
        /// Esta ação ocorre quando o utilizador preenche e envia o formulario existente na view candidaturaSantander
        /// Esta ação pretende verificação dos dados submetidos e guardar esta mesma informação
        /// </summary>
        /// <param name="model">modelo de dados de uma candidatura</param>
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
                    return View("../Erros/ErroSelecionarInstituicoes", perfil); // caso não selecione nada
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
                    return View("../Erros/ErroPrazos", perfil);
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
                return View("../Erros/ErroCandidatura", perfil);
            }
            //Erro
            return View("Erro");

        }
        /// <summary>
        /// O objectivo desta ação é visualizar a candidatura que foi previamente submetida
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
            
        }
        /// <summary>
        /// O objectivo desta ação é alterar uma candidatura previamente submetida.
        /// Esta ação só esta disponivel para o candidato caso o tecnico tenha verificado a candidatura e esta
        /// se encontre incompleta em algum campo da mesma
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
        /// Esta ação ocorre após serem submetidos dados atraves do formulário AlterarCandidaturaErasmus ou AlterarCandidaturaErasmus
        /// </summary>
        /// <param name="model">Modelo com dados alterados da candidatura</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AlterarCandidatura(CandidaturaViewModel model) {
            if (ModelState.IsValid) {
                List<string> x = Request.Form["lista"].ToList();
                if (x.Count <= 0)
                    return View("../Erros/ErroSelecionarInstituicoes"); // caso não selecione nada
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

                if (_gestor.AlterarCandidatura(user, candidatura, model).Result)
                    return View("AlterarCandidaturaSucesso"); // correu tudo bem
                return View("Erro"); //Erro
            }

            return View("Erro");// algo errado no model state
        }
        /// <summary>
        /// O objectivo desta ação é mostrar uma view com informação dos programas de mobilidade
        /// </summary>
        /// <returns></returns>
        public IActionResult ProgramasMobilidade()
        {
            Utilizador user = _gestor.getUtilizador(this.User).Result;
            Perfil p1 = _gestor.getPerfil(user);
            return View(p1);
        }
        /// <summary>
        /// O objectivo desta ação é mostrar uma view com informação das bolsas disponiveis
        /// </summary>
        /// <returns></returns>
        public IActionResult Bolsas()
        {
            Utilizador user = _gestor.getUtilizador(this.User).Result;
            Perfil p1 = _gestor.getPerfil(user);
            return View(p1);
        }
        /// <summary>
        /// O objectivo desta ação é mostrar uma view com informação dos membros do grupo
        /// </summary>
        /// <returns></returns>
        public IActionResult SobreNos()
        {
            Utilizador user = _gestor.getUtilizador(this.User).Result;
            Perfil p1 = _gestor.getPerfil(user);
            return View(p1);
        }
        /// <summary>
        /// O objectivo desta ação é dar ao utilizador a possibilidade de marcar uma reuniao com qualquer tecnico
        /// </summary>
        /// <returns></returns>
        public IActionResult MarcarReuniao()
        {
            return View();
        }
        /// <summary>
        /// Esta ação ocorre apos serem submetidos dados atraves do formulario da view MarcarReuniao
        /// </summary>
        /// <param name="viewModel">Modelo de dados de uma Reuniao</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult MarcarReuniao(MarcarReuniaoViewModel viewModel)
        {
            Utilizador actual = _gestor.getUtilizador(this.User).Result;
            Perfil perfil = _gestor.getPerfil(actual);
            bool marcou = _gestor.MarcarReuniao(actual, viewModel.DataReuniaoInicio, viewModel.DataReuniaoFim);
            if (marcou)
                return View("MarcarReuniaoSucesso", perfil); // marcou reuniao
            return View("../Erros/ErroMarcarReuniao", perfil);//erro nao marcou    
        }

        /// <summary>
        /// O objectivo desta ação é ler e retornar todos os eventos associados a um id de Perfil
        /// </summary>
        /// <param name="idPerfil">identificador de perfil</param>
        /// <returns>Json de lista de eventos</returns>
        [HttpGet]
        public JsonResult GetEvents(int idPerfil)
        {
            var events = _contexto.Eventos.Where(Eventos => Eventos.PerfilFK == idPerfil).ToList();
            return Json(events);
        }
        /// <summary>
        /// Esta ação tem como objetivo guardar um Evento 
        /// </summary>
        /// <param name="evento">Modelo de dados um evento</param>
        /// <returns>Json de sucesso</returns>
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
        /// Esta ação tem como objectivo apagar um eventos com um determinado identificador
        /// </summary>
        /// <param name="id">identificador do evento</param>
        /// <returns>Json com estado da operação</returns>
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
