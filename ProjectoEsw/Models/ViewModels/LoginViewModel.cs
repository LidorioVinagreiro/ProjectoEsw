using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectoEsw.Models.Identity
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="É obrigatório inserir Email")]
        [EmailAddress(ErrorMessage ="Este Email está incorrecto")]
        public string Email { get; set; }

        [Required(ErrorMessage ="")]
        [DataType(DataType.Password,ErrorMessage ="")]
        public string Password { get; set; }
    }
}
