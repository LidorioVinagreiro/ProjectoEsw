using ProjectoEsw.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectoEsw.Sprint_2_features.Models
{
    public enum Estado {APROVADA,INCOMPLETA,REJEITADA }
    public class Candidatura
    {
        public int ID { get; set; }
        public string NumeroEmergencia { get; set; }
        public string NomeEmergencia { get; set; }
        public string AfiliacaoEmergencia { get; set; }
        public int IBAN { get; set; }
        public int AnoCurricular { get; set; }
        public bool Bolsa { get; set; }
        public string CartaMotivacao { get; set; }
        public Estado Estado { get; set; }
        [ForeignKey("TipoCandidatura")]
        public int TipoCandidaturaFK { get; set; }
        [ForeignKey("Utilizador")]
        public string UtilizadorFK { get; set; }
        public virtual Utilizador Candidato { get; set; }
    }
}
