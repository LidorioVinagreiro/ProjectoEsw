using ProjectoEsw.Models;
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

        public string BuscarAjudaCampo(string pagina, string campo) {
            return null;
        }

        public string BuscarAjudaPagina(string pagina) {
            return null;
        }
    }
}
