using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectoEsw.Sprint_2_features.Models
{
    public class Instituicao
    {
        public int ID { get; set; }
        public string NomeInstituicao { get; set; }
        public string PaisInstituicao { get; set; }
        public string SiteInstituicao { get; set; }
        public bool Interno { get; set; }
        public string LongitudeInstituicao { get; set; }
        public string LatitudeInstituicao { get; set; }

    }
}
