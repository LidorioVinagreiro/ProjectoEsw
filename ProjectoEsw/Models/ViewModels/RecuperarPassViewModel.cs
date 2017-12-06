using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectoEsw.Models.ViewModels
{
    public class RecuperarPassViewModel
    {
        [Required(ErrorMessage = "Obrigatório inserir Email")]
        [EmailAddress(ErrorMessage = "Email incorrecto")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Obrigatório inserir NIF")]
        [Display(Name ="NIF")]
        public int Nif { get; set; }
    }
}
