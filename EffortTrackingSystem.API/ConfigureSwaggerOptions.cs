using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EffortTrackingSystem.API
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider apiVersionDescriptionProvider; // That instance gets the information that describe the discovered API versions information within our application.
        private readonly IWebHostEnvironment webHostEnvironment;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider apiVersionDescriptionProvider, IWebHostEnvironment webHostEnvironment)
        {
            this.apiVersionDescriptionProvider = apiVersionDescriptionProvider;
            this.webHostEnvironment = webHostEnvironment;
        }

        public void Configure(SwaggerGenOptions options)
        {
            // Create the swagger documents dynamically based on the the current api versions that we have.
            foreach (var desc in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {

                // Add a Swagger document (that is an open API specification).
                // The first parameter will be the name of the URI on which the "Open API specification" (Swagger document) can be found.
                // The second parameter will be an object of Microsoft.OpenApi.Models.OpenApiInfo, which will contain informations about that "Open API specification document" (Swagger document) 

                options.SwaggerDoc(desc.GroupName, new OpenApiInfo()
                {
                    Title = $"EffortTrackingSystem API {desc.ApiVersion}",
                    Version = desc.ApiVersion.ToString()
                });

                
                // Tell Swashbuckle where it can find Xml Comments so it can incorporate them into the OpenAPI specification document.
                //options.IncludeXmlComments(Path.Combine(webHostEnvironment.ContentRootPath, "EffortTrackingSystem.API.xml"));
            }

            // Add support for JWT token in swagger

            // That will generate a pop up that we will see once we click on the authorization button.
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {

                Description = "JWT Authorization header using the Bearer scheme." +
                "\r\n\r\n Enter 'Bearer' [space] and then your token in the text input below." +
                "\r\n\r\n Example: \"Bearer 12345abcdf\"",

                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"

            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement() {

                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }

            });
        }
    }
}
