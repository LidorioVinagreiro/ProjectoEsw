using ProjectoEsw.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectoEsw.Models.Candidatura_sprint2
{
    public enum Estado {APROVADA,INCOMPLETA,REJEITADA,EM_ANALISE }
    public class Candidatura
    {
        public int ID { get; set; }
        [DisplayName("Numero")]
        public string NumeroEmergencia { get; set; }
        [DisplayName("Nome")]
        public string NomeEmergencia { get; set; }
        [DisplayName("Afiliação")]
        public string AfiliacaoEmergencia { get; set; }
        [DisplayName("Curso")]
        public string CursoFrequentado { get; set; }
        public string Escola { get; set; }
        public int IBAN { get; set; }
        [DisplayName("Ano Curricular")]
        public int AnoCurricular { get; set; }
        public bool Bolsa { get; set; }
        [DisplayName("Carta Motivação")]
        public string CartaMotivacao { get; set; }
        public Estado Estado { get; set; }
        [ForeignKey("TipoCandidatura")]
        public int TipoCandidaturaFK { get; set; }
        public virtual TipoCandidatura TipoCandidatura { get; set; }
        [ForeignKey("Utilizador")]
        public string UtilizadorFK { get; set; } 
        public virtual Utilizador Candidato { get; set; } 
        public ICollection<Instituicoes_Candidatura> Instituicoes { get; set; }

    }
}
