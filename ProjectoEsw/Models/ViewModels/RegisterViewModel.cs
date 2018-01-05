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

        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$", ErrorMessage = "Insira uma Email valido")]
        [Required(ErrorMessage = "Obrigatório inserir Email")]
        [EmailAddress(ErrorMessage = "Email incorrecto")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [RegularExpression("[a-zA-Z. ]{2,20}", ErrorMessage = "O seu nome esta no formato incorrecto")]
        [Required(ErrorMessage = "Obrigatório inserir nome completo")]
        [Display(Name = "Nome Completo")]
        public string NomeCompleto { get; set; }

        [RegularExpression("[0-9]{2}/[0-9]{2}/[0-9]{4}", ErrorMessage = "A data de nascimento esta incorrecta")]
        [Required(ErrorMessage = "Obrigatório inserir data de nascimento válida")]
        [Display(Name = "Data Nascimento")]
        [DataType(DataType.Date)]
        public DateTime DataNasc { set; get; }

        [Required(ErrorMessage = "Obrigatório inserir Genero")]
        public Genero Genero { get; set; }

        [Required(ErrorMessage = "Obrigatório inserir Nacionalidade")]
        public Nacionalidade Nacionalidade { get; set; }

        [MinLength(5, ErrorMessage = "Insira uma Rua valida")]
        [Required(ErrorMessage = "Obrigatório inserir a rua")]
        [Display(Name = "Rua")]
        public string MoradaRua { get; set; }

        [Required(ErrorMessage = "Obrigatório inserir Concelho")]
        [Display(Name = "Concelho")]
        public Concelho MoradaConcelho { get; set; }


        [Required(ErrorMessage = "Obrigatório inserir Distrito")]
        [Display(Name = "Distrito")]
        public Distrito MoradaDistrito { get; set; }

        [RegularExpression("[0-9]{4}-[0-9]{3}", ErrorMessage = "Insira o codigo postal correcto no formato (0000-000) composto só por digitos")]
        [Required(ErrorMessage = "Obrigatório inserir o codigo postal")]
        [Display(Name = "Codigo Postal")]
        public string MoradaCodigoPostal { get; set; }
        
        [RegularExpression("[0-9]{9}",  ErrorMessage = "NIF Incorrecto (Deve conter so numeros com 9 digitos")]
        [Display(Name = "NIF")]
        [Required(ErrorMessage = "Obrigatório inserir NIF")]
        public int Nif { get; set; }

        [RegularExpression("[0-9]{9}", ErrorMessage = "O numero de Identificação incorrecto (Deve conter so numeros e ter 9 digitos)")]
        [Display(Name = "Numero de Identificação")]
        [Required(ErrorMessage = "Obrigatório inserir o Numero de Identificação")]
        public int NumeroIdentificacao { get; set; }

        [RegularExpression("(9)[1236]{1}[0-9]{7}", ErrorMessage = "O numero de telefone incorrecto (Deve conter so numeros e ter 9 digitos")]
        [Required(ErrorMessage = "Obrigatório inserir número de telefone/telemóvel")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "Obrigatório inserir password")]
        [DataType(DataType.Password, ErrorMessage = "Password deve ter 8 caracteres dos quais 1 letra maiuscula, 1 letra minuscula, 1 digito e um caracter especial (!@#%&)")]
        [Display(Name = "Palavra-chave")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Obrigatório confirmar password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Palavra-chave")]
        [Compare("Password", ErrorMessage = "A password não coincide")]
        public string ConfirmPassword { get; set; }
    }

   
}