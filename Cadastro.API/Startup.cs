using Cadastro.Core.Domain.Repositories;
using Cadastro.Core.Domain.Services;
using Cadastro.Core.Persistence.Contexts;
using Cadastro.Core.Persistence.Repositories;
using Cadastro.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Cadastro.API
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

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cadastro.API", Version = "v1" });
            });

            //services.AddDbContext<CadastroContext>(options =>
            //{
            //    options.UseInMemoryDatabase("loja-api-in-memory");
            //});

            //services.AddDbContext<CadastroContext>(options =>
            //        options.UseSqlServer("Server=DESKTOP-S3R5UB7\\SQLEXPRESS;Database=DesBd_CADUN;Trusted_Connection=True;"));

            // inje��o depend�ncia DBContext			
            services.AddDbContext<CadastroContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("CadastroConnection")));

            // inje��o depend�ncia Services
            services.AddScoped<IPessoaRepository, PessoaRepository>();
            services.AddScoped<IPessoaService, PessoaService>();
            services.AddScoped<IEnderecoRepository, EnderecoRepository>();
            services.AddScoped<IEnderecoService, EnderecoService>();
            services.AddScoped<ICepRepository, CepRepository>();
            services.AddScoped<ICepService, CepService>();
            services.AddScoped<IPessoaJuridicaRepository, PessoaJuridicaRepository>();
            services.AddScoped<IPessoaJuridicaService, PessoaJuridicaService>();
            services.AddScoped<IPessoaFisicaRepository, PessoaFisicaRepository>();
            services.AddScoped<IPessoaFisicaService, PessoaFisicaService>();
            services.AddScoped<IFilialRepository, FilialRepository>();
            services.AddScoped<IFilialService, FilialService>();

            //inje��o depend�ncia Services
            //services.AddTransient<IPessoaService, PessoaService>();
            //services.AddTransient<IPessoaRepository, PessoaRepository>();
            //services.AddTransient<ICepService, CepService>();
            //services.AddTransient<ICepRepository, CepRepository>();

            //inje��o de depend�ncia NewsoftJson - Microsoft.AspNetCore.Mvc.NewtonsoftJson
            services.AddControllers()
                    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling =
                     Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cadastro.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
