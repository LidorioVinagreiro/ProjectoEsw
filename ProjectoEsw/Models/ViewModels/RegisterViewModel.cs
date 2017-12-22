using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

/*
 * Fazer Validacoes Dos campos no registo e afins
 * Adicionar Nif Registo
 * Ver porque nao aparece datetime no modizila firefox
 * Fazer section links para o menu lateral do Shared layout
 * ao recuperar password fazer redirect to uma pagina com a confirmacao
 */

namespace ProjectoEsw.Models.Identity
{

    public class RegisterViewModel
    {
       

        [Required(ErrorMessage = "Obrigatório inserir Email")]
        [EmailAddress(ErrorMessage = "Email incorrecto")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Obrigatório inserir nome completo")]
        [Display(Name = "Nome Completo")]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "Obrigatório inserir data de nascimento válida")]
        [Display(Name = "Data Nascimento")]
        [DataType(DataType.Date)]
        public DateTime DataNasc { set; get; }
        [Required(ErrorMessage = "Obrigatório inserir data de nascimento válida")]
        public Genero Genero { get; set; }
        [Required(ErrorMessage = "Obrigatório inserir data de nascimento válida")]

        public Nacionalidade Nacionalidade { get; set; }
        [Display(Name = "Rua")]
        public string MoradaRua { get; set; }
        [Display(Name = "Concelho")]
        public Concelho MoradaConcelho { get; set; }
        [Display(Name = "Distrito")]
        public Distrito MoradaDistrito { get; set; }
        [Display(Name = "Codigo Postal")]
        public string MoradaCodigoPostal { get; set; }

        [Display(Name = "NIF")]
        //[Required(ErrorMessage = "Obrigatório inserir NIF")]
        public int Nif { get; set; }
        [Display(Name = "Numero de Identificação")]
        //[Required(ErrorMessage = "Obrigatório inserir o Numero de Identificação")]
        public int NumeroIdentificacao { get; set; }

        //[Required(ErrorMessage = "Obrigatório inserir número de telefone/telemóvel")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "Obrigatório inserir password")]
        [DataType(DataType.Password, ErrorMessage = "Password deve ter 8 caracteres dos quais 1 letra maiuscula, 1 letra minuscula, 1 digito e um caracter especial (!@#%&)")]
        [Display(Name = "Palavra-chave")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Obrigatório confirmar password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Palavra-chave")]
        [Compare("Password", ErrorMessage = "A password não coecide")]
        public string ConfirmPassword { get; set; }
    }

   
}