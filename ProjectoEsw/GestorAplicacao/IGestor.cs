using ProjectoEsw.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectoEsw.GestorAplicacao
{
    interface IGestor
    {
       Task<int> adicionarPerfilAsync(RegisterViewModel model);
    }
}
