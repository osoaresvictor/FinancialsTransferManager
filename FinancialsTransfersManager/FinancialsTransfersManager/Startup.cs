using FinancialsTransfersManager.Infra.MongoDB;
using FinancialsTransfersManager.Infra.RabbitMQ;
using FinancialsTransfersManager.Services.Transactions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System;

namespace FinancialsTransfersManager
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;

            var elasticUri = Configuration["ElasticConfiguration:Uri"];

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
                {
                    AutoRegisterTemplate = true,
                })
            .CreateLogger();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ITransactionRegistersRepository, TransactionRegistersRepository>();

            services.AddTransient<IProcessTransaction, ProcessTransaction>();
            services.AddTransient<IGetTransactionStatus, GetTransactionStatus>();
            services.AddTransient<IRabbitMQProducerClient, RabbitMQProducerClient>();

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Financials Transfer Manager - Acesso Test API",
                        Version = "v1",
                        Description = "Exemplo de API REST criada com ASP.NET Core + Mongo + ELK + RabbitMQ + Docker",
                        Contact = new OpenApiContact { Name = "Victor Soares", Email = "victor_soares@live.com" }
                    }
                );
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Financials Transfer Manager - Acesso Test API");
                c.RoutePrefix = string.Empty;
            });

            loggerFactory.AddSerilog();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
