using ProjectoEsw.Controllers;
using System;
using Xunit;
using ProjectoEsw.GestorAplicacao;
using ProjectoEsw.Models.Identity;
using Microsoft.AspNetCore.Identity;
using ProjectoEsw.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectoEsw.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using ProjectoEsw.GestorAplicacao;
using ProjectoEsw.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using ProjectoEsw.Models;
namespace TestesUnitarios
{
    public class UnitTest1
    {
        private UserManager<Utilizador> user;
        private SignInManager<Utilizador> ManegerS;
        private AplicacaoDbContexto _context;
        [Fact]
        public void Test1()
        {
            GestorEmail gestorEmail = new GestorEmail();
            Gestor gestor = new Gestor();
            
            
            var controller = new HomeController(UserManager<Utilizador>, ManegerS, _context);
        }
    }
}
