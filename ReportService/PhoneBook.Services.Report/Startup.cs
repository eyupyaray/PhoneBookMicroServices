using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using PhoneBook.Services.Report.Consumers;
using PhoneBook.Services.Report.Services;
using PhoneBook.Services.Report.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBook.Services.Report
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
            services.AddMassTransit(x =>
            {
                x.AddConsumer<ReportResultMessageCommandConsumer>();
                //default port: 5672
                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(Configuration["RabbitMQUrl"], "/", host =>
                    {

                        host.Username("guest");
                        host.Password("guest");
                    });

                    config.ReceiveEndpoint("report-result-message-service", e =>
                    {
                        e.ConfigureConsumer<ReportResultMessageCommandConsumer>(context);
                    });
                });
            });

            services.AddMassTransitHostedService();

            services.AddScoped<IReportService, ReportService>();
            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();

            services.Configure<DatabaseSettings>(Configuration.GetSection("DatabaseSettings"));

            services.AddSingleton<IDatabaseSettings>(db =>
            {
                return db.GetRequiredService<IOptions<DatabaseSettings>>().Value;

            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PhoneBook.Services.Report", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PhoneBook.Services.Report v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
