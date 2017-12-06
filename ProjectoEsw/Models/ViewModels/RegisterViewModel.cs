using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectoEsw.Models.Identity
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Obrigatório inserir Email")]
        [EmailAddress(ErrorMessage = "Email incorrecto")]
        [Display(Name ="Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Obrigatório inserir nome completo")]
        [Display(Name ="Nome Completo")]
        public string NomeCompleto { get; set; }

        //[Required(ErrorMessage = "Obrigatório inserir data de nascimento válida")]
        [Display(Name = "Data Nascimento")]
        public DateTime DataNasc { set; get; }

        [Display(Name ="NIF")]
        //[Required(ErrorMessage = "Obrigatório inserir NIF")]
        public int Nif { get; set; }
        [Display(Name = "Numero de Identificação")]
        //[Required(ErrorMessage = "Obrigatório inserir o Numero de Identificação")]
        public int NumeroIdentificacao { get; set; }

        public string Morada { get; set; }

        //[Required(ErrorMessage = "Obrigatório inserir número de telefone/telemóvel")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "Obrigatório inserir password")]
        [DataType(DataType.Password, ErrorMessage = "Password deve ter 8 caracteres dos quais 1 letra maiuscula, 1 letra minuscula, 1 digito e um caracter especial (!@#%&)")]
        [Display(Name = "Palavra-chave")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Obrigatório confirmar password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Palavra-chave")]
        [Compare("Password",ErrorMessage ="A password não coecide")]
        public string ConfirmPassword { get; set; }
    }
}
