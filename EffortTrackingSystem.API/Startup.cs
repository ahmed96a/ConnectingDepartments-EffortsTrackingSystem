using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EffortTrackingSystem.API.Hubs;
using EffortTrackingSystem.API.Models;
using EffortTrackingSystem.API.Models.Mapper;
using EffortTrackingSystem.API.Models.Repository;
using EffortTrackingSystem.API.Models.Repository.IRepository;
using EffortTrackingSystem.API.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EffortTrackingSystem.API
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
            services.AddControllers(); //Add Controllers

            // Register services to dependency Injection Container
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMissionRepository, MissionRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddTransient<EmailHelper>();

            // Configure EF Core
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            
            // Configure Identity System.
            services.AddIdentity<ApplicationUser, IdentityRole>(options => {

                // Configure the settings of Identity system.
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            // Configure AutoMapper
            services.AddAutoMapper(typeof(EffortTrackingSystemMapping));

            // Configure Api Versioning
            services.AddApiVersioning(options => {

                options.AssumeDefaultVersionWhenUnspecified = true; // This means that if you don't specify an api version, it will load the default version for you and will not give an error.
                options.DefaultApiVersion = new ApiVersion(1, 0); // Set the default version for api.
                options.ReportApiVersions = true; // Sets a value indicating whether requests report the current API version in the response header.

            });


            // Configure VersionedApiExplorer (Used to integrate versioning in Swagger), And set version format of the ApiExplorer. (we set it to be small 'v' letter and then the version number '1.5')
            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");

            // Configure Swagger Generator
            services.AddSwaggerGen();

            // Add Swagger Generator Configuration File
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();


            // Configure JWT Bearer Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options => {

                // Reference for all options:- https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.jwtbearer.jwtbeareroptions?view=aspnetcore-3.0

                //options.RequireHttpsMetadata = false;
                //options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters // used to set or get the parameters used to validate the token.
                {
                    // Reference for all properties of TokenValidationParameters :- https://docs.microsoft.com/en-us/dotnet/api/microsoft.identitymodel.tokens.tokenvalidationparameters?view=azure-dotnet

                    ValidateIssuer = true, // Enable validation of the Issuer of the token.
                    ValidateAudience = true, // Enable validation of the audience of the token.
                    ValidIssuer = Configuration["JWT:ValidIssuer"], // specify the issuer to validate against.
                    ValidAudience = Configuration["JWT:ValidAudience"], // specify the audience to validate against.
                    ValidateIssuerSigningKey = true, // Enable validation of the Issuer Signing Key (Our Secret Key in appsetting.json).
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"])) // Specify the key to validate.
                    
                    #region ValidateIssuerAudience When deploy our application or when we using a production environment
                    // 1- Once we deploy our application or when we using a production environment,
                    //    we can set 'ValidateIssuer' to true, and we can set the domain name of the issuer that we want to validate. The same thing for 'ValidateAudience' and the domain name of the audience.
                    #endregion
                };

                // Notification Module (Configure server side signalR authentication with JWT)
                // -----------------------------------

                // We have to hook the OnMessageReceived event in order to
                // allow the JWT authentication handler to read the access
                // token from the query string when a WebSocket or 
                // Server-Sent Events request comes in.

                // Sending the access token in the query string is required due to
                // a limitation in Browser APIs. We restrict it to only calls to the
                // SignalR hub in this code.
                // See https://docs.microsoft.com/aspnet/core/signalr/security#access-token-logging
                // for more information about security considerations when using
                // the query string to transmit the access token.

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/NotificationHub")))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };

                // -----------------------------------
            });

            // Add Policy
            services.AddAuthorization(options => {
                options.AddPolicy("UserPolicy", policy => {
                    policy.RequireRole("User");
                });
            });

            #region CORS
            services.AddCors();
            #endregion

            // Add SignalR
            services.AddSignalR(o => o.EnableDetailedErrors = true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseSwagger(); // Add Swagger middleware to serve the generated Swagger OAS (Docs) as a JSON endpoint.

            // We use Swagger UI, to create Documentation User Interface for the OpenAPI sepecification document.
            app.UseSwaggerUI(options => {

                // Configure the Swagger UI Endpoints (each Endpoint represent a document) dynamically using the apiVersionDescriptionProvider.
                foreach (var desc in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.GroupName.ToUpperInvariant()); // Add Swagger Json Endpoints.
                }

            });

            app.UseRouting();

            #region CORS
            // Add support for cross origin resource sharing (CORS, also known as cross domain requests)
            // It is a mechanism that uses additional Http headers to tell a browser to give an application running at one origin access to resources from a different origin.
            // And we do that because the URLs for our Web API (Backend) and Web Application (Frontend) are different (they have different origins).
            // That Error "Cross-Origin Request Blocked" is happen when a request is send from the client (browser) directly to the Web API. That error doesn't happen if the browser call the web api through HttpClient.

            app.UseCors(configurePolicy =>
            {

                configurePolicy.WithOrigins(Configuration["FrontProject:URL"])
                               .AllowAnyHeader()
                               .WithMethods("GET", "POST")
                               .AllowCredentials();
            });

            #endregion

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // Call MapControllers inside UseEndpoints to map attribute routed controllers.

                // Add SignalR routing
                endpoints.MapHub<NotificationHub>("/NotificationHub");
            });
        }
    }
}
