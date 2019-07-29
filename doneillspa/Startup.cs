﻿using doneillspa.DataAccess;
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
using doneillspa.Services.Email;
using doneillspa.Services.Calendar;
using doneillspa.Services.Document;
using doneillspa.Services;
using doneillspa.Services.Holiday;

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
            //services.AddDbContext<ApplicationContext>(opt => opt.UseSqlServer(Configuration["Data:Baby:ConnectionString"], providerOptions => providerOptions.CommandTimeout(60)));
            services.AddDbContext<ApplicationContext>(opt => opt.UseSqlServer(Configuration["ConnectionStrings:ConnectionString"], providerOptions => providerOptions.CommandTimeout(60)));
            
            services.AddScoped<IProjectRepository>(_ => new ProjectRepository(_.GetService<ApplicationContext>()));
            services.AddScoped<IClientRepository>(_ => new ClientRepository(_.GetService<ApplicationContext>()));
            services.AddScoped<ITimesheetRepository>(_ => new TimesheetRepository(_.GetService<ApplicationContext>()));
            services.AddScoped<IRateRepository>(_ => new RateRepository(_.GetService<ApplicationContext>()));
            services.AddScoped<ITimesheetEntryRepository>(_ => new TimesheetEntryRepository(_.GetService<ApplicationContext>()));
            services.AddScoped<ICertificationRepository>(_ => new CertificationRepository(_.GetService<ApplicationContext>()));
            services.AddScoped<IHolidayRequestRepository>(_ => new HolidayRequestRepository(_.GetService<ApplicationContext>()));
            services.AddScoped<INotificationRepository>(_ => new NotificationRepository(_.GetService<ApplicationContext>()));
            services.AddScoped<INoteRepository>(_ => new NoteRepository(_.GetService<ApplicationContext>()));
            services.AddScoped<INonChargeableTimeRepository>(_ => new NonChargeableTimeRepository(_.GetService<ApplicationContext>()));

            //Email Services
            services.AddScoped<IEmailService>(_ => new SendGridEmailService(Configuration));
            //services.AddScoped<IEmailService>(_ => new GmailMailService(Configuration));

            //Document Service
            services.AddScoped<IDocumentService>(_ => new DropBoxDocumentService(Configuration));

            //Calendar Services
            services.AddScoped<ICalendarService>(_ => new GoogleCalendarService(Configuration));

            //Setup Holiday Service and inject the required repositories
            services.AddScoped<IHolidayService>(_ => new HolidayService(_.GetService<IHolidayRequestRepository>()));

            services.AddSingleton<IJwtFactory, JwtFactory>();
            services.AddMvc();
            services.AddCors();

            //Enable Session
            services.AddSession();

            services.AddIdentity<ApplicationUser, IdentityRole<System.Guid>>()
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("AuthenticatedUser", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });


            var _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters()
               {
                   IssuerSigningKey = _signingKey,
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

            app.UseCors(builder =>
                builder.WithOrigins("http://doneillspa.azurewebsites.net"));

            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseAuthentication();
            app.UseSession();

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
