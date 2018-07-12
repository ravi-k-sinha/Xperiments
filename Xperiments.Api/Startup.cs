using System.Collections.Generic;
using System.IO;
using LendFoundry.Foundation.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;

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
            services.AddHttpServiceLogging("Xperiments");
            services.AddMvc();

            services.AddSwaggerGen(
                c =>
                {
                    c.SwaggerDoc("v1", new Info
                    {
                        Title = "Simple API for experiments",
                        Version = "v1",
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

            app.UseRequestLogging();
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
