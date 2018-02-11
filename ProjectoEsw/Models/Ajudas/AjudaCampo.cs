using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectoEsw.Models.Ajudas
{
    /// <summary>
    /// Modelo de dados de uma Ajuda de campo
    /// </summary>
    public class AjudaCampo
    {
        /// <summary>
        /// Identidicador do campo
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Identificador de campo de AjudaPagina
        /// </summary>
        [ForeignKey("AjudaPagina")]
        public int? PaginaFK { get; set; }
        /// <summary>
        /// Nome do campo
        /// </summary>
        public string Campo { get; set; }
        /// <summary>
        /// Descrição do campo
        /// </summary>
        public string Descricao { get; set; }
    }
}
