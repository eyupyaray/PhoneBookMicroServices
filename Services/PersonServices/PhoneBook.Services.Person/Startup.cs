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
using PhoneBook.Services.Person.Consumers;
using PhoneBook.Services.Person.Services;
using PhoneBook.Services.Person.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBook.Services.Person
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
                x.AddConsumer<ReportDetailMessageCommandConsumer>();
                //default port: 5672
                x.UsingRabbitMq((context, config) =>
                {
                    config.Host(Configuration["RabbitMQUrl"], "/", host =>
                    {

                        host.Username("guest");
                        host.Password("guest");
                    });

                    config.ReceiveEndpoint("report-message-service", e =>
                    {
                        e.ConfigureConsumer<ReportDetailMessageCommandConsumer>(context);
                    });
                });
            });

            services.AddMassTransitHostedService();

            services.AddScoped<IContactTypeService, ContactTypeService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddAutoMapper(typeof(Startup));

            services.AddControllers();

            services.Configure<DatabaseSettings>(Configuration.GetSection("DatabaseSettings"));

            services.AddSingleton<IDatabaseSettings>(db =>
            {
                return db.GetRequiredService<IOptions<DatabaseSettings>>().Value;

            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PhoneBook.Services.Person", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PhoneBook.Services.Person v1"));
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
