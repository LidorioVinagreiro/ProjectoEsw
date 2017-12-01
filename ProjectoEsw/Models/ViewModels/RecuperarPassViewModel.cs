using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectoEsw.Models.ViewModels
{
    public class RecuperarPassViewModel
    {
        [Required(ErrorMessage ="")]
        [EmailAddress(ErrorMessage ="")]
        public string Email { get; set; }
        public int Nif { get; set; }
    }
}
