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
        public string Email { get; set; }
        [Required(ErrorMessage = "Obrigatório inserir NIF")]
        public int Nif { get; set; }
    }
}
