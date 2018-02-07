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
        public int ID { get; set; }
        [RegularExpression("(9)[1236]{1}[0-9]{7}", ErrorMessage = "O numero de telefone incorrecto (Deve conter so numeros e ter 9 digitos")]
        [Required(ErrorMessage = "Obrigatório inserir número de telemóvel, para concluir a candidatura")]
        public string NumeroEmergencia { get; set; }
        [Required(ErrorMessage = "Obrigatório inserir o nome da pessoa a contactar em caso de emergência, para concluir a candidatura")]
        public string NomeEmergencia { get; set; }
        [Required(ErrorMessage = "Obrigatório especificar a afiliação que essa pessoa tem consigo, para concluir a candidatura")]
        public string AfiliacaoEmergencia { get; set; }
        [Required(ErrorMessage = "Obrigatório inserir o curso que frequenta, para concluir a candidatura")]
        public string CursoFrequentado { get; set; }
        [Required(ErrorMessage = "Obrigatório inserir o nome da escola que frequenta, para concluir a candidatura")]
        public string Escola { get; set; }
        [Required(ErrorMessage = "Obrigatório inserir o IBAN da sua conta, para concluir a candidatura.")]
        [RegularExpression(@"a-zA-Z]{2}[0-9]{23}")]
        public int IBAN { get; set; }
        [Required(ErrorMessage = "obrigatório inserir o ano curricular que frequenta, para concluir a candidatura")]
        [Range(1, 5)]
        public int AnoCurricular { get; set; }
        public bool Bolsa { get; set; }
        //[Required(ErrorMessage = "Obrigatório inserir uma carta de motivação para")]
        public string CartaMotivacao { get; set; }
        [Required(ErrorMessage = "Necessario escolher instituições para onde se quer candidatar")]
        public List<Instituicao> Instituicoes { get; set; }
        public string UtilizadorFK { get; set; }
        public virtual Utilizador Candidato { get; set; }
    }
}
