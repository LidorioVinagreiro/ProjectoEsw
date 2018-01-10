using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectoEsw.Models.Candidatura
{
    public class Curso
    {
        public int ID { get; set; }
        public string NomeCurso { get; set; }
        public string TipoCurso { get; set; }
        public int NVagas { get; set; }
    }
}
