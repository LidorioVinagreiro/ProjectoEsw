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

        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$", ErrorMessage = "Insira uma Email valido" )]
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

        [RegularExpression("[0-9]{4}-[0-9]{3}")]
        [Required(ErrorMessage = "Obrigatório inserir o codigo postal")]
        [Display(Name = "Codigo Postal")]
        public string MoradaCodigoPostal { get; set; }

        [Range(100000000, 999999999, ErrorMessage = "Insira uma numero de identificacao com 9 digitos")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Insira só digitos")]
        [Display(Name = "NIF")]
        [Required(ErrorMessage = "Obrigatório inserir NIF")]
        public int Nif { get; set; }

        [Range(100000000, 999999999, ErrorMessage = "Insira uma numero de identificacao com 9 digitos")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "O numero de Identificação deve ser so numeros")]
        [Display(Name = "Numero de Identificação")]
        [Required(ErrorMessage = "Obrigatório inserir o Numero de Identificação")]
        public int NumeroIdentificacao { get; set; }
        [Range(910000000,969999999, ErrorMessage = "Insira um numero de telefone valido")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "O numero de telefone deve ser so numeros")]
        [Required(ErrorMessage = "Obrigatório inserir número de telefone/telemóvel")]
        public string Telefone { get; set; }

        [RegularExpression("[a-zA-Z0-9]{8,15}", ErrorMessage = "Tem de inserir uma password com 8 caracteres no minimo, e 1 digito e 1 caracter " )]
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