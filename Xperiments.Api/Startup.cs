﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using LendFoundry.Foundation.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using Xperiments.Middleware;
using Xperiments.Persistence;
using Xperiments.Persistence.Common;
using Xperiments.Repository;
using Xperiments.Service;

namespace Xperiments.Api
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

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IPersonService, PersonService>();
            services.AddTransient<IPersonRepository, PersonRepository>();
            
            services.AddSingleton<IMongoConfiguration>(p =>
                new MongoConfiguration(Settings.MongoConnectionString, Settings.MongoDatabaseName));
            
            var appVersion = typeof(Program).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
            Environment.SetEnvironmentVariable("APP_VERSION", appVersion, EnvironmentVariableTarget.Process);

            services.AddHttpServiceLogging("Xperiments");
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ResponseStandardizationFilter));
            });


            services.AddSwaggerGen(
                c =>
                {
                    c.SwaggerDoc("v1", new Info
                    {
                        Title = "Simple API for experiments",
                        Version = $"v1 ({Environment.GetEnvironmentVariable("APP_VERSION")})",
                        Description = "A sample API to experiment with ASP.NET features",
                        TermsOfService = "UBS Confidential",
                        Contact = new Contact
                        {
                            Name = "John Doe",
                            Email = "johndoe@in.unibiz.com",
                            Url = "http://www.unibiz.com/blogs/jdoe"
                        },
                        License = new License
                        {
                            Name = "Apache 2.0",
                            Url = "http://www.apache.org/licenses/LICENSE-2.0.html"
                        }

                    });

                    c.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                    {
                        Type = "apiKey",
                        Name = "Authorization",
                        Description = "For accessing the API a valid JWT token must be passed in all the " +
                                      "queries in the 'Authorization' header. The syntax used in the 'Authorization' " +
                                      "header should be Bearer xxxxx.yyyyyy.zzzz",
                        In = "header"
                    });

                    c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                    {
                        { "Bearer", new string[]{ }}
                    });

                    c.DescribeAllEnumsAsStrings();
                    c.IgnoreObsoleteProperties();
                    c.DescribeStringEnumsInCamelCase();
                    c.IgnoreObsoleteActions();

                    c.TagActionsBy(apiDesc => apiDesc.HttpMethod);
                    c.OrderActionsBy(apiDesc => apiDesc.RelativePath);

                    var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "Xperiments.Api.xml");
                    c.IncludeXmlComments(filePath);
                }
            );


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRequestLoggingMiddleware();
            //app.UseRequestLogging();
            app.UseMvc();

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api-docs/simple/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api-docs/simple/v1/swagger.json", "Sample API v1");
                c.RoutePrefix = "api-docs"; // default is 'swagger'
            });

        }
    }
}
