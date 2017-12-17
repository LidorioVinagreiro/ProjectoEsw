using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectoEsw.Models.Identity
{
    public class Perfil
    {
        public int ID { get; set; }
        [DisplayName("Nome Completo")]
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public int Nif { get; set; }
        [DisplayName("Numero de Identificação")]
        public int NumeroIdentificacao { get; set; }
        [DisplayName("Data Nascimento")]
        public DateTime DataNasc { get; set; }
        public string Morada { get; set; }
        public string Telefone { get; set; }
        public string Foto { get; set; }
        [ForeignKey("Utilizador")]
        public string UtilizadorFK { get; set; }
        public virtual Utilizador Utilizador { get; set; } 
    }
}
