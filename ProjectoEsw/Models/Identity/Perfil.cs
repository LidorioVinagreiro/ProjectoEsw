using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectoEsw.Models.Identity
{
    public enum Genero {
        MASCULINO,
        FEMENINO
    };
    public enum Nacionalidade {
        PORTUGUESA,
        INGLESA,
        ESPANHOLA
    };
    public enum Concelho {
        Alcácer_do_Sal,
        Alcochete,
        Almada,
        Barreiro,
        Grândola,
        Moita,
        Montijo,
        Palmela,
        Santiago_do_Cacém,
        Seixal,
        Sesimbra,
        Setúbal,
        Sines
    };
    public enum Distrito {
        Aveiro,
        Beja,
        Braga,
        Bragança,
        Castelo_Branco,
        Coimbra,
        Évora,
        Faro,
        Guarda,
        Leiria,
        Lisboa,
        Portalegre,
        Porto,
        Santarém,
        Setúbal,
        Viana_do_Castelo,
        Vila_Real,
        Viseu
        };

    public class Perfil
    {
        public int ID { get; set; }
        [DisplayName("Nome Completo")]
        public string NomeCompleto { get; set; }
        public Genero Genero { get; set; }
        public Nacionalidade Nacionalidade { get; set; }
        public string Email { get; set; }
        public int Nif { get; set; }
        [DisplayName("Numero de Identificação")]
        public int NumeroIdentificacao { get; set; }
        [DisplayName("Data Nascimento")]
        public DateTime DataNasc { get; set; }

        public string MoradaRua { get; set; }
        public string MoradaConcelho { get; set; }
        public string MoradaDistrito { get; set; }
        public string MoradaCodigoPostal { get; set; }

        public string Telefone { get; set; }
        public string Foto { get; set; }
        [ForeignKey("Utilizador")]
        public string UtilizadorFK { get; set; }
        public virtual Utilizador Utilizador { get; set; } 
    }
}
