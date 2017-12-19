using ProjectoEsw.Models;
using ProjectoEsw.Models.Ajudas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectoEsw.GestorAplicacao
{
    public class GestorAjudas
    {
        private AplicacaoDbContexto _context;
        public GestorAjudas(AplicacaoDbContexto context)
        {
            _context = context;
        }

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
