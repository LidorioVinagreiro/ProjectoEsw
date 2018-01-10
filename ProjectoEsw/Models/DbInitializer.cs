using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProjectoEsw.Models.Identity;
using ProjectoEsw.Models.Ajudas;
using ProjectoEsw.Models.Candidatura_sprint2;

namespace ProjectoEsw.Models
{
    public static class DbInitializer
    {
        public static void Initialize(AplicacaoDbContexto context,UserManager<Utilizador> userManager)
        {

            if (!context.Instituicoes.Any()) {
                context.Instituicoes.Add(new Instituicao
                {
                    Interno = true,
                    LatitudeInstituicao = "n/a",
                    LongitudeInstituicao = "n/a",
                    NomeInstituicao = "IPS Setubal",
                    PaisInstituicao = "Portugal",
                    SiteInstituicao = "wwww.google.pt",
                });
                context.Instituicoes.Add(new Instituicao
                {
                    Interno = true,
                    LatitudeInstituicao = "n/a",
                    LongitudeInstituicao = "n/a",
                    NomeInstituicao = "IPS Barreiro",
                    PaisInstituicao = "Portugal",
                    SiteInstituicao = "wwww.google.pt",
                });

                context.Instituicoes.Add(new Instituicao
                {
                    Interno = false,
                    LatitudeInstituicao = "n/a",
                    LongitudeInstituicao = "n/a",
                    NomeInstituicao = "IPS Espanha",
                    PaisInstituicao = "Espanha",
                    SiteInstituicao = "wwww.espanha.com",
                });

                context.Instituicoes.Add(new Instituicao
                {
                    Interno = false,
                    LatitudeInstituicao = "n/a",
                    LongitudeInstituicao = "n/a",
                    NomeInstituicao = "Franca IPS",
                    PaisInstituicao = "Franca",
                    SiteInstituicao = "wwww.Franca.com",
                });

                context.Instituicoes.Add(new Instituicao
                {
                    Interno = false,
                    LatitudeInstituicao = "n/a",
                    LongitudeInstituicao = "n/a",
                    NomeInstituicao = "IPS Inglaterra",
                    PaisInstituicao = "Inglaterra",
                    SiteInstituicao = "wwww.inglaterra.com",
                });
            }

            if (!context.TipoCandidatuas.Any()) {
                context.TipoCandidatuas.Add(new TipoCandidatura
                {
                    Tipo = "Erasmus",
                    DataInicio = System.DateTime.Now,
                    DataFim = System.DateTime.Now.AddMonths(4)
                });

                context.TipoCandidatuas.Add(new TipoCandidatura
                {
                    Tipo = "Santander",
                    DataInicio = System.DateTime.Now,
                    DataFim = System.DateTime.Now.AddMonths(4)
                });
                context.SaveChanges();
            }
            if (!context.Roles.Any())
            {
                context.Roles.Add(new IdentityRole { Name = "Candidato", NormalizedName = "CANDIDATO" });
                context.Roles.Add(new IdentityRole { Name = "Tecnico", NormalizedName = "TECNICO" });
                context.Roles.Add(new IdentityRole { Name="Administrador", NormalizedName="ADMINISTRADOR"});
                context.SaveChanges();
            }

            if (userManager.FindByEmailAsync("tecnico1@est.pt").Result == null)
            {
                //    //PasswordHasher<Utilizador> hash = new PasswordHasher<Utilizador>();
                Utilizador tec1 = new Utilizador { UserName = "tecnico1@est.pt" ,EmailConfirmed=true};
                Utilizador tec2 = new Utilizador { UserName = "tecnico2@est.pt" ,EmailConfirmed=true};
                Utilizador tec3 = new Utilizador { UserName = "tecnico3@est.pt" ,EmailConfirmed=true};
                Utilizador admin = new Utilizador { UserName = "admin@est.pt" ,EmailConfirmed=true};
                try
                {
                    userManager.CreateAsync(tec1, "tecnico1");
                    userManager.CreateAsync(tec2, "tecnico2");
                    userManager.CreateAsync(tec3, "tecnico3");
                    userManager.CreateAsync(admin, "admin");
                    context.SaveChangesAsync();
                }catch(Exception e) {

                }

                var id = context.Roles.Where(row => row.Name.Equals("Tecnico")).Single().Id;
                var id1 = context.Roles.Where(row => row.Name.Equals("Administrador")).Single().Id;
                context.UserRoles.Add(new IdentityUserRole<string> { RoleId = id, UserId = tec1.Id });
                context.UserRoles.Add(new IdentityUserRole<string> { RoleId = id, UserId = tec2.Id });
                context.UserRoles.Add(new IdentityUserRole<string> { RoleId = id, UserId = tec3.Id });
                context.UserRoles.Add(new IdentityUserRole<string> { RoleId = id1, UserId = admin.Id });
                context.SaveChanges();
            }

            if (!context.AjudaPaginas.Any())
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

                context.AjudaPaginas.Add(new AjudaPagina
                {
                    Pagina = "Home.EditarPerfil",
                    Descricao = "Esta página tem como função a alteração dos campos no perfil do utilizador. " +
                            "Poderá escolher alterar o perfil ou as password, para tal terá de inserir os campos necessarios á sua conclusão" +
                            " que estao especificados na pagina"
                });
                context.AjudaPaginas.Add(new AjudaPagina
                {
                    Pagina = "Candidato.CandidaturaEramus",
                    Descricao = "Esta página tem como função a submição da Candidatura Erasmus+. " +
                            "Tera de preencher os campos necessarios a candidatura, que serão os seus dados pessoais, os dados da pessoa que será o seu contacto de emergencia" +
                            " e os dados referentes ao seu estatuto escolar"
                });
                context.AjudaPaginas.Add(new AjudaPagina
                {
                    Pagina = "Candidato.CandidaturaSantander",
                    Descricao = "Esta página tem como função a submição da Candidatura Santander. " +
                            "Tera de preencher os campos necessarios a candidatura, que serão os seus dados pessoais, os dados da pessoa que será o seu contacto de emergencia" +
                            " e os dados referentes ao seu estatuto escolar"
                });
                context.AjudaPaginas.Add(new AjudaPagina
                {
                    Pagina = "Candidato.Perfil",
                    Descricao = "Esta página tem como função a submição da Candidatura Erasmus+. " +
                            "Tera de preencher os campos necessarios a candidatura, que serão os seus dados pessoais, os dados da pessoa que será o seu contacto de emergencia" +
                            " e os dados referentes ao seu estatuto escolar"
                });
                context.SaveChanges();

            }

            if (!context.AjudaCampos.Any())
            {
                context.AjudaCampos.Add(new AjudaCampo { PaginaFK = 1, Campo = "Email", Descricao = "Deverá inserir o seu email pessoal do tipo(ex: jbvc@hotmail.com)"});
                context.AjudaCampos.Add(new AjudaCampo { PaginaFK = 1, Campo = "Password", Descricao = "Neste campo deverá inserir a sua password com letras maiusculas, letras minusculas e digitos" });

                context.AjudaCampos.Add(new AjudaCampo { PaginaFK = 2, Campo = "Nome Completo", Descricao = "Neste campo deverá inserir um nome completo com apenas letras" });
                context.AjudaCampos.Add(new AjudaCampo { PaginaFK = 2, Campo = "Data Nascimento", Descricao = "Neste campo deverá inserir a data nascimento no formato dia/mês/ano" });
                context.AjudaCampos.Add(new AjudaCampo { PaginaFK = 2, Campo = "Numero Telefone", Descricao = "Neste campo deverá inserir o seu numero telefone com 9 digitos numericos" });
                context.AjudaCampos.Add(new AjudaCampo { PaginaFK = 2, Campo = "Email", Descricao = "Neste campo deverá inserir o seu email do tipo(ex: jbvc@hotmail.com)" });
                context.AjudaCampos.Add(new AjudaCampo { PaginaFK = 2, Campo = "Password", Descricao = "Neste campo deverá definir a sua password com letras maiusculas, letras minusculas e digitos" });
                context.AjudaCampos.Add(new AjudaCampo { PaginaFK = 2, Campo = "Confirmar Password", Descricao = "Neste campo deverá inserir novamente a password definida anteriormente" });
                context.AjudaCampos.Add(new AjudaCampo { PaginaFK = 2, Campo = "Morada Rua", Descricao = "Neste campo deverá inserir a rua da sua morada (Ex. Rua aberto delgado)" });
                context.AjudaCampos.Add(new AjudaCampo { PaginaFK = 2, Campo = "Morada Concelho", Descricao = "Neste campo deverá inserir o concelho da sua morada " });
                context.AjudaCampos.Add(new AjudaCampo { PaginaFK = 2, Campo = "Morada Distrito", Descricao = "Neste campo deverá inserir o codigo de postal da sua morada " });
                context.AjudaCampos.Add(new AjudaCampo { PaginaFK = 2, Campo = "Morada Codigo Postal", Descricao = "Neste campo deverá inserir o seu codigo postal (Ex. 0000-000)" });
                context.AjudaCampos.Add(new AjudaCampo { PaginaFK = 2, Campo = "Genero", Descricao = "Neste campo deverá inserir a nova morada para o seu perfil" });
                context.AjudaCampos.Add(new AjudaCampo { PaginaFK = 2, Campo = "Nacionalidade", Descricao = "Neste campo deverá inserir a sua nacionalidade" });
                context.AjudaCampos.Add(new AjudaCampo { PaginaFK = 2, Campo = "Numero Identificação", Descricao = "Neste campo deverá inserir o seu numero de identificação da escola com apenas digitos" });
                context.AjudaCampos.Add(new AjudaCampo { PaginaFK = 2, Campo = "NIF", Descricao = "Neste campo deverá inserir o seu numero de identificação fiscal que deverá conter 9 digitos numéricos" });

                context.AjudaCampos.Add(new AjudaCampo { PaginaFK = 3, Campo = "Email", Descricao = "Neste campo deverá inserir o seu email do tipo(ex: jbvc@hotmail.com)" });

                context.AjudaCampos.Add(new AjudaCampo { PaginaFK = 4, Campo = "Novo Numero Identificação", Descricao = "Neste campo deverá inserir o seu novo numero de identificação da escola com apenas digitos" });
                context.AjudaCampos.Add(new AjudaCampo { PaginaFK = 4, Campo = "Novo NIF", Descricao = "Neste campo deverá inserir o seu novo numero de identificação fiscal que deverá conter 9 digitos numéricos" });
                context.AjudaCampos.Add(new AjudaCampo { PaginaFK = 4, Campo = "Nova Morada Rua", Descricao = "Neste campo deverá inserir a sua nova rua da sua morada (Ex. Rua aberto delgado) " });
                context.AjudaCampos.Add(new AjudaCampo { PaginaFK = 4, Campo = "Nova Morada Concelho", Descricao = "Neste campo deverá inserir o novo concelho da sua morada" });
                context.AjudaCampos.Add(new AjudaCampo { PaginaFK = 4, Campo = "Nova Morada Distrito", Descricao = "Neste campo deverá inserir a novo distrito da sua morada" });
                context.AjudaCampos.Add(new AjudaCampo { PaginaFK = 4, Campo = "Nova Morada Codigo Postal", Descricao = "Neste campo deverá inserir o novo codigo postal da sua morada" });
                context.AjudaCampos.Add(new AjudaCampo { PaginaFK = 4, Campo = "Novo Telefone", Descricao = "Neste campo deverá inserir o seu novo numero de telefone" });
                context.AjudaCampos.Add(new AjudaCampo { PaginaFK = 4, Campo = "Nova Password", Descricao = "Neste campo deverá definir a sua nova password com letras maiusculas, letras minusculas e digitos" });
                context.AjudaCampos.Add(new AjudaCampo { PaginaFK = 4, Campo = "Confirmar Nova Password", Descricao = "Neste campo deverá inserir novamente a sua nova password definida anteriormente" });
                context.SaveChanges();
            }

        }

            
        }
    }


