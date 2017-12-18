using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectoEsw.Models.Ajudas
{
    public class AjudaCampo
    {
        public int ID { get; set; }
        [ForeignKey("AjudaPagina")]
        public int? PaginaFK { get; set; }
        public string Campo { get; set; }
        public string Descricao { get; set; }
    }
}
