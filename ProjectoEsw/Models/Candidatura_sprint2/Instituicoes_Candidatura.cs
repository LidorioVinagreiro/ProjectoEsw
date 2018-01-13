using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectoEsw.Models.Candidatura_sprint2
{
    public class Instituicoes_Candidatura
    {
        public int CandidaturaId { get; set; }
        public virtual Candidatura Candidatura { get; set; }
        public int InstituicaoId { get; set; }
        public virtual Instituicao Instituicao { get; set; }
    }
}
