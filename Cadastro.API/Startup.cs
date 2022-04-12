using Cadastro.Data;
using Cadastro.Data.EFC;
using Cadastro.Data.EFC.Repositories;
using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Contracts.Services;
using Cadastro.Domain.Contracts.UnitOfWorks;
using Cadastro.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

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
            //Serviço de Contexto			
            services.AddDbContext<CadastroContextEFC>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("CadastroConnection")));

            //Serviços de Repositorio
            services.AddScoped<CadastroContextEFC>();
            services.AddTransient<IPessoaRepository, PessoaRepositoryEFC>();
            services.AddTransient<IEnderecoRepository, EnderecoRepositoryEFC>();
            services.AddTransient<IEnderecoPessoaRepository, EnderecoPessoaRepositoryEFC>();
            services.AddTransient<ICepRepository, CepRepositoryEFC>();
            services.AddTransient<IEmpresaRepository, EmpresaRepositoryEFC>();
            services.AddTransient<IPessoaFisicaRepository, PessoaFisicaRepositoryEFC>();
            services.AddTransient<IFilialRepository, FilialRepositoryEFC>();
            services.AddTransient<ISocioRepository, SocioRepositoryEFC>();

            //Serviço de Unidade de Trabalho
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            //Serviços da Aplicação
            services.AddTransient<IPessoaService, PessoaService>();
            services.AddTransient<IEnderecoService, EnderecoService>();
            services.AddTransient<ICepService, CepService>();
            services.AddTransient<IEmpresaService, EmpresaService>();

            //Controllers protegidos contra acesso anônimo exceto as actions que tenham o atributo
            services.AddControllersWithViews(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                       .RequireAuthenticatedUser()
                       .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            //Serviço de Mapeamento
            services.AddAutoMapper(typeof(Startup).Assembly);

            //Serviço NewsoftJson - Microsoft.AspNetCore.Mvc.NewtonsoftJson
            services.AddControllers()
                    .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling =
                        Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                    .AddNewtonsoftJson(options => options.SerializerSettings.NullValueHandling =
                        Newtonsoft.Json.NullValueHandling.Ignore);

            //Authentication JwtBearer
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                var key = Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]);

                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    // Validar a terceira parte do token jwt usando o segredo que adicionamos
                    // no appsettings e verifica se o token jwt foi gerado
                    // https://www.browserling.com/tools/random-string <- Gera o segredo aleatoriamente
                    ValidateIssuerSigningKey = true,

                    // Adiciona chave secreta à nossa criptografia Jwt
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cadastro.API", Version = "v1" });
            });
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

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
