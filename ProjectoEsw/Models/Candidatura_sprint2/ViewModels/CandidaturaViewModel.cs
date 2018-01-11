using ProjectoEsw.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectoEsw.Models.Candidatura_sprint2.ViewModels
{
    public class CandidaturaViewModel
    {
        public int ID { get; set; }
        public string NumeroEmergencia { get; set; }
        public string NomeEmergencia { get; set; }
        public string AfiliacaoEmergencia { get; set; }
        public string CursoFrequentado { get; set; }
        public string Escola { get; set; }
        public int IBAN { get; set; }
        public int AnoCurricular { get; set; }
        public bool Bolsa { get; set; }
        public string CartaMotivacao { get; set; }
        public List<Instituicao> Instituicoes;
        public string UtilizadorFK { get; set; } 
        public virtual Utilizador Candidato { get; set; }
    }
}
