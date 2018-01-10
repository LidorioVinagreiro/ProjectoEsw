using ProjectoEsw.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectoEsw.Models.Candidatura
{
    public class Candidatura
    {
        public int ID { get; set; }
        public string UtilizadorFK { get; set; }
        public virtual Utilizador Candidato { get; set; }
        public string NomeEmergencia { get; set; }
        public string Afiliacao { get; set; }
        public string telefone { get; set; }
        public string Iban { get; set; }
        public string Escola { get; set; }
        public string CursoFrequentado { get; set; }
        public int AnoCurricular { get; set; }
        public bool Bolsa { get; set; }
        public string InstituicoesFrequentar { get; set; }
        public string CartaMotivacao { get; set; }
    }
}
