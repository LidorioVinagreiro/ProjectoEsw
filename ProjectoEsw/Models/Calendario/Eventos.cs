using ProjectoEsw.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectoEsw.Models.Calendario
{
    /// <summary>
    /// Tipo de Evento
    /// </summary>
    public enum Tipo { Entrevista, Reuniao, Pessoal};
    /// <summary>
    /// Core de um eventos
    /// </summary>
    public enum Cores { Vermelho, Amarelo , Verde};
    /// <summary>
    /// Modelo de dados de eventos
    /// Esta modelo é usado no calendario da aplicação
    /// </summary>
    public class Eventos
    {
        /// <summary>
        /// identificador unico de evento
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Titulo de evento
        /// </summary>
        public string Titulo { get; set; }
        /// <summary>
        /// Descrição de evento
        /// </summary>
        public string Descricao { get; set; }
        /// <summary>
        /// Data de inicio do evento
        /// </summary>
        public DateTime Inicio { get; set; }
        /// <summary>
        /// Data de fim do evento
        /// </summary>
        public DateTime Fim { get; set; }
        public bool DiaTodo { get; set; }

        public Tipo Tipo { get; set; }
        public Cores CorEvento { get; set; }
        /// <summary>
        /// identificador de perfil associado ao evento
        /// </summary>
        [ForeignKey("Perfil")]
        public int? PerfilFK { get; set; }
        public virtual Perfil Utilizador { get; set; }

        [ForeignKey("Perfil")]
        public int? EntrevistadorFK { get; set; }
        public virtual Perfil Entrevistador { get; set; }

    }

}
