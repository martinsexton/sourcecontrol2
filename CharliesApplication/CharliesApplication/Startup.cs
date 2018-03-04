using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CharliesApplication.Models;
using CharliesApplication.DataAccess;

namespace CharliesApplication
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
            //AddDbContext adds the DbContext as a scoped service, meaning an instance is created per web request
            services.AddDbContext<BabyContext>(opt => opt.UseSqlServer(Configuration["Data:Baby:ConnectionString"], providerOptions => providerOptions.CommandTimeout(60)));
            //Repository added with an instance per request scope. i.e. instance of repository created for each web request
            services.AddScoped<IBabyRepository>(_ => new BabyRepository(_.GetService<BabyContext>()));
            services.AddScoped<IAppointmentRepository>(_ => new AppointmentRepository(_.GetService<BabyContext>()));
            services.AddScoped<IActivityRepository>(_ => new ActivityRepository(_.GetService<BabyContext>()));
            services.AddMvc().AddJsonOptions(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
