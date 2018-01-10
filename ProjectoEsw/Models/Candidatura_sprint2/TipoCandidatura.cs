﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectoEsw.Models.Candidatura_sprint2
{
    public class TipoCandidatura
    {
        public int ID { get; set; }
        public string Tipo { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        
        public IList<Candidatura> Candidaturas { get; set; }
    }
}