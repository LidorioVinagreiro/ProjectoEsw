using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectoEsw.Models.Identity
{

    public enum Genero
    {
        Masculino,
        Femenino
    };
    public enum Nacionalidade
    {
        Portuguesa,
        Inglesa,
        Espanhola
    };
    public enum Concelho
    {
        [Display(Name = "Alcácer do Sal")]
        Alcácer_do_Sal,
        Alcochete,
        Almada,
        Barreiro,
        Grândola,
        Moita,
        Montijo,
        Palmela,
        [Display(Name = "Santiago do Cacém")]
        Santiago_do_Cacém,
        Seixal,
        Sesimbra,
        Setúbal,
        Sines
    };
    public enum Distrito
    {
        Aveiro,
        Beja,
        Braga,
        Bragança,
        [Display(Name = "Castelo Branco")]
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
        [Display(Name = "Viana do Castelo")]
        Viana_do_Castelo,
        [Display(Name = "Vila Real")]
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
        [DataType(DataType.Date)]
        public DateTime DataNasc { get; set; }
        [DisplayName("Rua")]
        public string MoradaRua { get; set; }
        [DisplayName("Concelho")]
        public Concelho MoradaConcelho { get; set; }
        [DisplayName("Distrito")]
        public Distrito MoradaDistrito { get; set; }
        [DisplayName("Código Postal")]
        public string MoradaCodigoPostal { get; set; }

        public string Telefone { get; set; }
        public string Foto { get; set; }
        [ForeignKey("Utilizador")]
        public string UtilizadorFK { get; set; }
        public virtual Utilizador Utilizador { get; set; } 
    }
}
