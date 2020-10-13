using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Solution.Authenticar.Novo.Service;
using System.Text;

namespace Solution.Authenticar.Novo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public object Enconding { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers();

            var key = Encoding.ASCII.GetBytes(Settings.Secret());
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            //Pol�ticas Globais
            //Particularmente, em diversos cen�rios acho mais f�cil restringir TODA a API 
            //e liberar s� o que preciso, conforme a demanda.
            //Desta forma, podemos definir pol�ticas globais, dizendo que TODO endpoint da nossa API 
            //requer autentica��o.
            //Isto � feito tamb�m no ConfigureServices, no m�todo AddMvc, desta forma.
            //services.AddMvc(config =>
            //{
            //    var policy = new AuthorizationPolicyBuilder()
            //                    .RequireAuthenticatedUser()
            //                    .Build();
            //    config.Filters.Add(new AuthorizeFilter(policy));
            //}).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //Adicionando Autoriza��o
            //Se utilizamos o AddAuthentication para autentica��o, o AddAuthorization vai para autoriza��o. Intuitivo n�.
            //Neste cen�rio teremos dois Claims, um para usu�rio e outro para administrador, e estes s�o configurados neste formato, tamb�m no m�todo 
            //ConfigureServices da classe Startup.cs.
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("user", policy => policy.RequireClaim("Cadastro", "user"));
            //    options.AddPolicy("admin", policy => policy.RequireClaim("Cadastro", "admin"));
            //});

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
