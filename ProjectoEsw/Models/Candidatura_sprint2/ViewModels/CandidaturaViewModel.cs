using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectoEsw.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectoEsw.Models.Candidatura_sprint2.ViewModels
{
    public class CandidaturaViewModel
    {
        /// <summary>
        /// identificador unico de uma candidatura
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Numero de emergencia de um utilizador
        /// É obrigatorio
        /// Tem que cumprir uma expressão regular
        /// </summary>
        [RegularExpression("(9)[1236]{1}[0-9]{7}", ErrorMessage = "O numero de telefone incorrecto (Deve conter so numeros e ter 9 digitos")]
        [Required(ErrorMessage = "Obrigatório inserir número de telemóvel, para concluir a candidatura")]
        public string NumeroEmergencia { get; set; }
        /// <summary>
        /// Nome da afiliação de emergencia de um utilizador
        /// É obrigatorio
        /// </summary>
        [Required(ErrorMessage = "Obrigatório inserir o nome da pessoa a contactar em caso de emergência, para concluir a candidatura")]
        public string NomeEmergencia { get; set; }

        /// <summary>
        /// Tipo de afiliação de emergencia de um utilizador
        /// É obrigatorio
        /// </summary>
        [Required(ErrorMessage = "Obrigatório especificar a afiliação que essa pessoa tem consigo, para concluir a candidatura")]
        public string AfiliacaoEmergencia { get; set; }
        [Required(ErrorMessage = "Obrigatório inserir o curso que frequenta, para concluir a candidatura")]
        /// <summary>
        /// Nome de um curso de um utilizador
        /// É obrigatorio
        /// </summary>
        public string CursoFrequentado { get; set; }
        [Required(ErrorMessage = "Obrigatório inserir o nome da escola que frequenta, para concluir a candidatura")]
        /// <summary>
        /// Nome da Escola que um utilizador frequenta
        /// É obrigatorio
        /// </summary>
        public string Escola { get; set; }
        [Required(ErrorMessage = "Obrigatório inserir o IBAN da sua conta, para concluir a candidatura.")]
        [RegularExpression("(PT50)[0-9]{21}", ErrorMessage ="O IBAN tem de conter 23 digitos para ser valido")]
        /// <summary>
        /// Numero de IBAN de um utilizador
        /// É obrigatorio
        /// </summary>
        public string IBAN { get; set; }
        [Required(ErrorMessage = "obrigatório inserir o ano curricular que frequenta, para concluir a candidatura")]
        [Range(1, 5, ErrorMessage ="Erro")]
        /// <summary>
        /// Ano Curricular de um utilizador
        /// É obrigatorio
        /// E tem um valor minimo e maximo a cumprir
        /// </summary>
        public int AnoCurricular { get; set; }
        /// <summary>
        /// Se o utilizador pretende bolsa ou não
        /// </summary>
        public bool Bolsa { get; set; }
        /// <summary>
        /// Caminho para a carta de motivação
        /// </summary>
        public string CartaMotivacao { get; set; }
        /// <summary>
        /// Lista de instituições para a candidatura
        /// </summary>
        public List<Instituicao> Instituicoes { get; set; }
        /// <summary>
        ///Identificador unico do utilizador que efectuou a candidatura
        /// </summary>
        public string UtilizadorFK { get; set; }
        public virtual Utilizador Candidato { get; set; }
    }
}
