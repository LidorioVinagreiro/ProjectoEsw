using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectoEsw.Models.Ajudas
{
    /// <summary>
    /// Modelo de dados de uma ajuda de pagina
    /// </summary>
    public class AjudaPagina
    {
        /// <summary>
        /// Identificador da AjudaPagina
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Nome da pagina
        /// </summary>
        public string Pagina { get; set; }
        /// <summary>
        /// Descrição da pagina
        /// </summary>
        public string Descricao { get; set; }
    }
}
