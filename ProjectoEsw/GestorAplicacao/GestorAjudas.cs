using ProjectoEsw.Models;
using ProjectoEsw.Models.Ajudas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectoEsw.GestorAplicacao
{
    /// <summary>
    /// Esta classe tem como objectivo agregar todas as funções que 
    /// permitam procurar informações relativas a diferentes tipos de ajuda ao utilizador
    /// no uso da aplicação web
    /// </summary>
    public class GestorAjudas
    {
        /// <summary>
        /// Atributos privado da classe GestorAjudas
        /// </summary>
        private AplicacaoDbContexto _context;
        
        /// <summary>
        /// Todos os parametros sao recebidos por DI(Dependency injection)
        /// </summary>
        /// <param name="context">Parametro do tipo AplicacaoDbcontexto</param>
        public GestorAjudas(AplicacaoDbContexto context)
        {
            _context = context;
        }
        /// <summary>
        /// Esta função tem como objectivo procurar a ajuda de um determinado campo de uma pagina
        /// </summary>
        /// <param name="pagina">parametro do tipo string com nome da pagina web</param>
        /// <param name="campo">Parametro do tipo string com o nome do campo da pagina web</param>
        /// <returns>Retorna uma classe do tipo AjudaCampo</returns>
        public AjudaCampo BuscarAjudaCampo(string pagina, string campo) {
            AjudaPagina paginaAjuda = BuscarAjudaPagina(pagina);
            AjudaCampo queryCampo = (from AjudaCampo in _context.AjudaCampos
                                     where (AjudaCampo.Campo == campo && AjudaCampo.PaginaFK == paginaAjuda.ID)
                                     select new AjudaCampo {
                                         Campo = AjudaCampo.Campo,
                                         Descricao = AjudaCampo.Descricao
                                     }).FirstOrDefault();
            return queryCampo;
        }
        /// <summary>
        /// Esta função tem como objectivo procurar a ajuda para uma determinada pagina
        /// </summary>
        /// <param name="pagina">Parametro do tipo string com o nome da pagina</param>
        /// <returns></returns>
        public AjudaPagina BuscarAjudaPagina(string pagina) {
            AjudaPagina queryAjuda = (from AjudaPagina in _context.AjudaPaginas
                                       where AjudaPagina.Pagina == pagina
                                       select new AjudaPagina
                                       {
                                           ID = AjudaPagina.ID,
                                           Pagina = AjudaPagina.Pagina,
                                           Descricao = AjudaPagina.Descricao
                                       }).FirstOrDefault();
            return queryAjuda;
        }
    }
}
