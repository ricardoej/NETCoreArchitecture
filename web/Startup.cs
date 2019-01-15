using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using repository.core;
using service;
using service.core;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using web.Settings;

namespace web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<ApplicationContext>(optionsAction => optionsAction.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSingleton<IUnitOfWork, UnitOfWork>();
            AddServices(services, typeof(IRepository));
            AddServices(services, typeof(IService));

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            
            ConfigureJWTAuthentication(services);
        }

        private void ConfigureJWTAuthentication(IServiceCollection services)
        {
            var appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUsuarioService>();
                        var userId = int.Parse(context.Principal.Identity.Name);
                        var user = userService.Get(userId);
                        if (user == null)
                        {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
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
        }

        private static void AddServices(IServiceCollection services, Type baseInterface)
        {
            Assembly assembly = Assembly.GetAssembly(baseInterface);
            Type[] assemblyTypes = assembly.GetTypes();
            var interfacesExtendsBaseType = assemblyTypes.Where(t => t.IsInterface && !t.IsGenericType && t.GetInterfaces().Contains(baseInterface));
            foreach (var interfaceType in interfacesExtendsBaseType)
            {
                var concreteTypes = assemblyTypes.Where(t => t.IsClass && t.GetInterfaces().Contains(interfaceType));
                if (concreteTypes.Count() == 0)
                {
                    Console.Error.WriteLine($"Não existe nenhuma implementação para o tipo {interfaceType}");
                }
                else if (concreteTypes.Count() > 1)
                {
                    Console.Error.WriteLine($"Existem mais de uma implementação para o tipo {interfaceType} ({String.Join(", ", concreteTypes)})");
                }
                else
                {
                    var concreteType = concreteTypes.Single();
                    var method = typeof(ServiceCollectionServiceExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
                        .Where(m => m.Name == "AddTransient" && m.IsGenericMethod && m.GetGenericArguments().Length == 2 && m.GetParameters().Length == 1).Single();
                    MethodInfo generic = method.MakeGenericMethod(interfaceType, concreteType);
                    generic.Invoke(null, new object[] { services });
                }
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();
        }
    }
}
