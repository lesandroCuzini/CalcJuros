using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using CalcJuros.Domain.Service;
using CalcJuros.Domain.Service.Contracts;
using System.Net.Http;
using Polly.Extensions.Http;
using Microsoft.OpenApi.Models;

namespace CalcJuros.Api
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API Calculadora de Juros",
                    Description = "Implementação para demonstrar a utilização de recursos como CI/CD, Docker, Teste Unitário (TDD), Documentação de APIs (Swagger) e requisição em API externa. "+
                        "Toda estas implementações utilizando o Framework dotNet Core versão 3.1, linguagem de programação C#.",
                    Contact = new OpenApiContact
                    {
                        Name = "Lesandro de Assis Cuzini",
                        Email = "lesandro.assis@gmail.com",
                        Url = new Uri("https://github.com/lesandroCuzini")
                    }
                });
            });
            services.AddHttpClient<IInfoJurosService, InfoJurosServiceImpl>(client =>
                {
                    client.BaseAddress = new Uri( (Environment.GetEnvironmentVariable("INFO_JUROS_URL_BASE") != null ? 
                        Environment.GetEnvironmentVariable("INFO_JUROS_URL_BASE") : Configuration["InfoJurosUrlBase"]) );
                })
                    .AddPolicyHandler(GetRetryPolicy())
                    .ConfigurePrimaryHttpMessageHandler(configureHandler);
            services.AddTransient<ICalcJurosService, CalcJurosServiceImpl>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Calculadora de Juros v1.0");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        private HttpMessageHandler configureHandler ()
        {
            var validarCertificadoSSLHttpClient = (Environment.GetEnvironmentVariable("VALIDAR_SSL_HTTPCLIENT") != null ? 
                Convert.ToBoolean(Environment.GetEnvironmentVariable("VALIDAR_SSL_HTTPCLIENT")) : Configuration.GetValue<bool>("ValidarCertificadoSSLHttpClient"));
            var handler = new HttpClientHandler();
            if (!validarCertificadoSSLHttpClient)
            {
                handler.ServerCertificateCustomValidationCallback = (httpRequestMessage, x509Certificate2, x509Chain, sslPolicyErrors) =>
                {
                    return true;
                };
            }
            return handler;
        }
    }
}
