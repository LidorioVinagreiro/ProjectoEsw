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
            services.AddDbContext<AplicacaoDbContexto>(options => options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=RegistoTeste"));
            services.AddIdentity<AplicacaoUtilizador, IdentityRole>().AddEntityFrameworkStores<AplicacaoDbContexto>();
            services.AddTransient<IdentityUser, AplicacaoUtilizador>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseAuthentication();
            //AuthAppBuilderExtensions.UseAuthentication(app);

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Principal}/{action=Index}/");

                routes.MapRoute(
                    name: "Candidato",
                    template: "{ controller = TiposUtilizador/Candidato}/{ action = Index}/"
                    );
                routes.MapRoute(
                    name: "Tecnico",
                    template: "{ controller = TiposUtilizador/Tecnico}/{ action = Index}/"
                    );
                routes.MapRoute(
                    name: "Administrador",
                    template: "{ controller = TiposUtilizador/Administrador}/{ action = Index}/"
                    );
            });
        }
    }
}
