using doneillspa.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using doneillspa.Models;
using Microsoft.AspNetCore.Identity;
using doneillspa.Data;
using Microsoft.IdentityModel.Tokens;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using doneillspa.Auth;
using doneillspa.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;

namespace doneillspa
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
            services.AddDbContext<ApplicationContext>(opt => opt.UseSqlServer(Configuration["Data:Baby:ConnectionString"], providerOptions => providerOptions.CommandTimeout(60)));
            services.AddScoped<IProjectRepository>(_ => new ProjectRepository(_.GetService<ApplicationContext>()));
            services.AddSingleton<IJwtFactory, JwtFactory>();
            services.AddMvc();

            services.AddIdentity<ApplicationUser, IdentityRole<System.Guid>>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            var _keyByteArray = Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]);
            var _signingKey = new SymmetricSecurityKey(_keyByteArray);
            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = Configuration["Jwt:Issuer"];
                options.Audience = Configuration["Jwt:Audience"];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });


            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("ApiUser", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.ApiAccess)
            //    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​).RequireAuthenticatedUser().Build());
            //});

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("ApiUser", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser()
                    .RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.ApiAccess).Build());
            });

            var _keyByteArray2 = Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]);
            var _signingKey2 = new SymmetricSecurityKey(_keyByteArray);

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //  .AddJwtBearer(options =>
            //  {
            //      options.TokenValidationParameters = new TokenValidationParameters
            //      {
            //          ValidateIssuer = true,
            //          ValidateAudience = true,
            //          ValidateLifetime = true,
            //          ValidateIssuerSigningKey = true,
            //          ValidIssuer = Configuration["Jwt:Issuer"],
            //          ValidAudience = Configuration["Jwt:Audience"],
            //          IssuerSigningKey = _signingKey2
            //      };
            //  });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters()
               {
                   IssuerSigningKey = _signingKey2,
                   ValidAudience = Configuration["Jwt:Audience"],
                   ValidIssuer = Configuration["Jwt:Issuer"],
                   ValidateIssuerSigningKey = true,
                   ValidateLifetime = true,
                   ClockSkew = TimeSpan.FromMinutes(0)
               };
           });
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
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                    //spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });

            //Used to setup some default Identity users and roles
            //SeedData.Run(app.ApplicationServices).Wait();
        }
    }
}
