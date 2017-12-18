using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProjectoEsw.Models.Identity;
using ProjectoEsw.Models.Ajudas;

namespace ProjectoEsw.Models
{
    public static class DbInitializer
    {
        public static void Initialize(AplicacaoDbContexto context)
        {
            if (!context.Roles.Any())
            {
                context.Roles.Add(new IdentityRole { Name = "Candidato", NormalizedName = "CANDIDATO" });
                context.Roles.Add(new IdentityRole { Name = "Tecnico", NormalizedName = "TECNICO" });
                context.Roles.Add(new IdentityRole { Name="Administrador", NormalizedName="ADMINISTRADOR"});
                context.SaveChanges();
            }
           
            if(!context.AjudaPaginas.Any())
            {
                context.AjudaPaginas.Add(new AjudaPagina
               {Pagina="Home.Login",
                Descricao = "Nesta página o utilizador terá de prencher os campos email e password " +
                             "com os campos respetivos que definiu ao fazer o registo.De seguida premirá o botão sign in para que o " +
                             "login seja efetuado" });

                context.AjudaPaginas.Add(new AjudaPagina
                {Pagina = "Home.Register",
                 Descricao = "Nesta página o utilizador irá efetuar o seu registo inserindo os dados relativos ao Nome Completo, " +
                             "Data Nascimento, Numero identificação Fiscal, Numero Identificaçao, Numero Telefone, " +
                             "Email e Password, para que fique registado na página" });

                context.AjudaPaginas.Add(new AjudaPagina
                {Pagina = "Home.RecuperarPassword",
                 Descricao = "Esta página tem como função fazer a recuperação da password caso o utilizador ja nao se lembre qual é. " +
                             "O utilizador terá de inserir o seu email que preencheu no registo e carregar no botao recuperar e " +
                             "receberáuma mensagem com a sua password"});

              //  context.SaveChanges();

            }

            //if (!context.AjudaCampos.Any())
            //{
            //    context.AjudaCampos.Add(new AjudaCampo { PaginaFK = ,
            //        Campo = "Email", Descricao = "Deverá inserir o email que dediniu do tipo(ex: jbvc@hotmail.com)" });
            //    context.AjudaCampos.Add(new Ajudas.AjudaCampo { PaginaFK = login, Campo = "password", Descricao = "A passowrd deverá conter letra maiuscula, letra minuscula e numeros" });

            //    context.AjudaCampos.Add(new AjudaCampo { PaginaFK = register, Campo = "Nome Completo", Descricao = "Inserir um nome completo com apenas letras" });
            //    context.AjudaCampos.Add(new AjudaCampo { PaginaFK = register, Campo = "Data Nascimento", Descricao = "Inserir a data nascimento no formato mês/dia/ano" });
            //    context.AjudaCampos.Add(new AjudaCampo { PaginaFK = register, Campo = "NIF", Descricao = "Deverá inserir o seu numero de identificação fiscal que deverá conter 9 digitos numéricos" });
            //    context.AjudaCampos.Add(new AjudaCampo { PaginaFK = register, Campo = "Numero Identificação", Descricao = "Deverá inserir o seu numero de identificação com apenas digitos" });
            //    context.AjudaCampos.Add(new AjudaCampo { PaginaFK = register, Campo = "Numero Telefone", Descricao = "Deverá inserir o seu numero telfone com 9 digitos numericos" });
            //    context.AjudaCampos.Add(new AjudaCampo { PaginaFK = register, Campo = "email", Descricao = "Deverá inserir o seu email do tipo (ex: jbvc@hotmail.com)" });
            //    context.AjudaCampos.Add(new AjudaCampo { PaginaFK = register, Campo = "Password", Descricao = "Deverá definir a sua password com letras maiusculas, letras minusculas e digitos" });
            //    context.AjudaCampos.Add(new AjudaCampo { PaginaFK = register, Campo = "Confirmar password", Descricao = "everá inserir novamente a password definida anteriormente" });

            //   context.AjudaCampos.Add(new AjudaCampo { PaginaFK = recuperar password, Campo = "email", Descricao = "campo com o email do tipo (ex: jbvc@hotmail.com)" });
            //    context.SaveChanges();
            //}

        }

            
        }
    }


