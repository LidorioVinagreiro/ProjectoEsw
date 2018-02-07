using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectoEsw.Models;
using ProjectoEsw.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ProjectoEsw.GestorAplicacao;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.IO;

namespace ProjectoEsw
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var conect = "Server=(localdb)\\mssqllocaldb;Database=RegistoTeste;";
            var connect1 = Configuration.GetConnectionString("ProjectoEsw_grupo2");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.SupportedUICultures = new[] { new CultureInfo("pt-PT") };
                options.SupportedCultures = new [] { new CultureInfo("pt-PT") };
                options.DefaultRequestCulture = new RequestCulture("pt-PT");             
            });


            services.AddEntityFrameworkSqlServer()
            .AddDbContext<AplicacaoDbContexto>(options =>
            {
                options.UseSqlServer(connect1);
            });

            //services.AddDbContext<AplicacaoDbContexto>(options => options.UseSqlServer(conect));

            services.AddIdentity<Utilizador, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 4;
                    options.Password.RequiredUniqueChars = 0;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                }
            ).AddEntityFrameworkStores<AplicacaoDbContexto>()
            .AddDefaultTokenProviders();

            


            services.ConfigureApplicationCookie(options => 
            {
                options.LoginPath = "/Home/Login";
            }
            );
            
            services.AddTransient<IdentityUser, Utilizador>();
            services.AddTransient<Gestor>();
            services.AddTransient<GestorAjudas>();
            services.AddTransient<GestorEmail>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            string users = "\\Utilizadores";
            string path = env.WebRootPath + users;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            
            app.UseAuthentication();
            
            app.UseRequestLocalization();

            app.UseStaticFiles();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/");

                routes.MapRoute(
                    name: "Candidato",
                    template: "{controller=TiposUtilizador/Candidato}/{ action = Index}/"
                    );
                routes.MapRoute(
                    name: "Tecnico",
                    template: "{controller=TiposUtilizador/Tecnico}/{ action = Index}/"
                    );
                routes.MapRoute(
                    name: "Administrador",
                    template: "{ controller = TiposUtilizador/Administrador}/{ action = Index}/"
                    );


            });
            
        }
    }
}
