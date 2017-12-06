using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectoEsw.Models.Identity
{
    public class AplicacaoUtilizador : IdentityUser
    {
        [ForeignKey("Perfil")]
        public int PerfilFK { get; set; }
        //identityUser já contem informacao de utilizadores!
        public virtual Perfil Perfil { get; set; }
    }
}
