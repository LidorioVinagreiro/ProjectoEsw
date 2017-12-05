using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectoEsw.Models.Identity
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Obrigatório inserir Email")]
        [EmailAddress(ErrorMessage ="Email incorrecto")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Obrigatório inserir password")]
        [DataType(DataType.Password,ErrorMessage ="Password deve ter 8 caracteres dos quais 1 letra maiuscula, 1 letra minuscula, 1 digito e um caracter especial (!@#%&)")]
        public string Password { get; set; }
    }
}
