using ProjectoEsw.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectoEsw.Sprint_2_features.Models
{
    public class Candidatura
    {
        public int ID { get; set; }
        public string UtilizadorFK { get; set; }
        public virtual Utilizador Candidato { get; set; }
    }
}
