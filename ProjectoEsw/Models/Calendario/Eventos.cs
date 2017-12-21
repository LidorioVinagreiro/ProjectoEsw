using ProjectoEsw.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectoEsw.Models.Calendario
{
    public enum Tipo { Entrevista, Reuniao, Pessoal};
    public enum Cores { Vermelho, Amarelo , Verde};

    public class Eventos
    {
        public int ID { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public bool DiaTodo { get; set; }

        public Tipo Tipo { get; set; }
        public Cores CorEvento { get; set; }

        [ForeignKey("Perfil")]
        public int? PerfilFK { get; set; }
        public virtual Perfil Utilizador { get; set; }

        [ForeignKey("Perfil")]
        public int? EntrevistadorFK { get; set; }
        public virtual Perfil Entrevistador { get; set; }

    }

}
