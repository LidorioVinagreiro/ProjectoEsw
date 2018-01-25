using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ProjectoEsw.Models.Estatisticas_sprint3
{
    [DataContract]
    public class DataPoint
    {
        public DataPoint(string name, double value)
        {
            this.name = name;
            this.value = value;
        }
        [DataMember(Name = "label")]
        public string name { get; set; }
        [DataMember(Name = "y")]
        public double value { get; set; }

    }
}
