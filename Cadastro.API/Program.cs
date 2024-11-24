using Cadastro.Data.EFC;
using Cadastro.Data.EFC.Repositories;
using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Contracts.Services;
using Cadastro.Domain.Contracts.UnitOfWorks;
using Cadastro.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        //Serviço de Contexto DBContext
        builder.Services.AddDbContext<CadastroContextEFC>(options =>
        options.UseSqlServer(
                builder.Configuration.GetConnectionString("CadastroConnection")));

        //Serviços de Repositorio
        builder.Services.AddScoped<CadastroContextEFC>();
        builder.Services.AddTransient<IPessoaRepository, PessoaRepositoryEFC>();
        builder.Services.AddTransient<IEnderecoRepository, EnderecoRepositoryEFC>();
        builder.Services.AddTransient<IEnderecoPessoaRepository, EnderecoPessoaRepositoryEFC>();
        builder.Services.AddTransient<ICepRepository, CepRepositoryEFC>();
        builder.Services.AddTransient<IEmpresaRepository, EmpresaRepositoryEFC>();
        builder.Services.AddTransient<IPessoaFisicaRepository, PessoaFisicaRepositoryEFC>();
        builder.Services.AddTransient<IFilialRepository, FilialRepositoryEFC>();
        builder.Services.AddTransient<ISocioRepository, SocioRepositoryEFC>();

        //Serviço de Unidade de Trabalho
        builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

        //Serviços da Aplicação
        builder.Services.AddTransient<IPessoaService, PessoaService>();
        builder.Services.AddTransient<IEnderecoService, EnderecoService>();
        builder.Services.AddTransient<ICepService, CepService>();
        builder.Services.AddTransient<IEmpresaService, EmpresaService>();

        //Controllers protegidos contra acesso anônimo exceto as actions que tenham o atributo
        builder.Services.AddControllersWithViews(config =>
        {
            var policy = new AuthorizationPolicyBuilder()
                   .RequireAuthenticatedUser()
                   .Build();
            config.Filters.Add(new AuthorizeFilter(policy));
        });

        //Serviços de Mapeamento
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        //Injeção de dependência NewsoftJson - Microsoft.AspNetCore.Mvc.NewtonsoftJson
        builder.Services.AddControllers()
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddNewtonsoftJson(options => options.SerializerSettings.NullValueHandling =
                    Newtonsoft.Json.NullValueHandling.Ignore);

        // Evitar repetições de ciclos
        builder.Services.AddControllers().AddJsonOptions(x =>
           x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

        //Authentication JwtBearer
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(jwt =>
        {
            var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Secret"]!);

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

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        await GerarDB(app.Services);

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();

        async static Task GerarDB(IServiceProvider services)
        {
            using var db = services.CreateScope().ServiceProvider.GetRequiredService<CadastroContextEFC>();
            await db.Database.EnsureCreatedAsync();
            //await db.Database.MigrateAsync();
        }
    }
}