namespace Xperiments.Api
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using Amazon.S3;
    using LendFoundry.Foundation.Logging;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.PlatformAbstractions;
    using Middleware;
    using MongoDB.Bson.Serialization;
    using Persistence;
    using Persistence.Common;
    using Prometheus;
    using Repository;
    using Service;
    using Swashbuckle.AspNetCore.Swagger;

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
            
            BsonSerializer.RegisterSerializationProvider(new MyProvider());

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IPersonService, PersonService>();
            services.AddTransient<IPersonRepository, PersonRepository>();
            
            services.AddSingleton<IMongoConfiguration>(p =>
                new MongoConfiguration(Settings.MongoConnectionString, Settings.MongoDatabaseName));

            AddS3Dependencies(services);
            
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
                    c.CustomSchemaIds(x => x.FullName);
                    
                    c.TagActionsBy(apiDesc => apiDesc.HttpMethod);
                    c.OrderActionsBy(apiDesc => apiDesc.RelativePath);

                    var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "Xperiments.Api.xml");
                    c.IncludeXmlComments(filePath);

                    
                }
            );


        }

        private void AddS3Dependencies(IServiceCollection services)
        {
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddSingleton<IS3Service, S3Service>();
            services.AddAWSService<IAmazonS3>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMetricServer();
            app.UseRequestLoggingMiddleware();
            //app.UseRequestLogging();
            app.UseMvc();

            var virtualBasePathSegment = Environment.GetEnvironmentVariable("SWAGGER_VIRTUAL_BASEPATH_SEGMENT");
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api-docs/simple/{documentName}/swagger.json";
                c.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    swagger.BasePath = virtualBasePathSegment;
                });
            });
            
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../api-docs/simple/v1/swagger.json", "Sample API v1");
                c.RoutePrefix = "api-docs"; // default is 'swagger'
                c.DocumentTitle = "Xperiments API Documentation";
                c.EnableFilter();
                c.DisplayOperationId();
                c.EnableValidator();
            });
            
            /*app.UseReDoc(c =>
            {
                c.RoutePrefix = "api-docs";

                c.SpecUrl = "simple/v1/swagger.json";
                c.DocumentTitle = "Xperiments API Documentation";
            });*/
        }
    }
}
