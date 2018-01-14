using ProjectoEsw.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectoEsw.Models.Candidatura_sprint2.ViewModels
{
    public class MarcarEntrevistaViewModel
    {
        public string CandidatoID { get; set; }
        public Utilizador Candidato { get; set; }
        public DateTime DataEntrevistaInicio { get; set; }
        public DateTime DataEntrevistaFim { get; set; }
    }
}
