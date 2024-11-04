using System;
using System.Collections.Generic;
using System.Linq;
using AnagraficaAPI.Configuration;
using AnagraficaAPI.Domain.Configuration;
using AnagraficaAPI.Domain.Mapping;
using AnagraficaAPI.Domain.Services;
using AnagraficaAPI.Entity.Context;
using AnagraficaAPI.Mapping;
using AnagraficaAPI.Middleware;
using DocsGeneratorService;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Poseidon.AuthorizationManager;
using Serilog;
using ServiceUtilities.SystemInfo;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;
using ZeusApi.Middleware;

namespace AnagraficaAPI
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Log.Information("Startup::ConfigureServices");

            try
            {
                AppSettings appSettings = _configuration.GetSection(AppSettings.AppSettingsName).Get<AppSettings>();
                SecuritySettings securitySettings = _configuration.GetSection(SecuritySettings.SecuritySettingsName).Get<SecuritySettings>();
                services.AddConfig(_configuration);

                if (securitySettings.EnableIdentityServer)
                {
                    services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
                    {
                        options.Authority = securitySettings.AuthorityUrl;
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters.ValidateIssuer = false;
                        options.TokenValidationParameters.ValidateIssuerSigningKey = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateAudience = false
                        };

                    });

                    services.AddAuthorization(options =>
                    {
                        options.AddPolicy("ApiScope", policy =>
                        {
                            policy.RequireAuthenticatedUser();
                            policy.RequireClaim("scope", securitySettings.RequiredScope);
                        });
                    });
                }

                services.AddControllers()
                    .AddFluentValidation(options =>
                    {
                        options.RegisterValidatorsFromAssemblyContaining<Startup>();
                    });

                //Add DB Context
                services.AddDbContext<AnagraficaContext>(optContext =>
                {
                    optContext.UseSqlServer(_configuration.GetConnectionString("TreePlatConnection"), optSql =>
                    {
                        optSql.AddResillientStrategy(EnabledSqlEnableRetryOnFailure: appSettings.EnabledSqlEnableRetryOnFailure);
                    });
                    optContext.LogTo(
                        filter: (eventId, level) => eventId.Id == CoreEventId.ExecutionStrategyRetrying,
                        logger: (eventData) =>
                        {
                            var retryEventData = eventData as ExecutionStrategyEventData;
                            var exceptions = retryEventData.ExceptionsEncountered;
                            Log.Fatal($"Retry #{exceptions.Count} with delay {retryEventData.Delay} due to error: {exceptions.Last().Message}");
                        });
                });

                //Add Automapper config profile
                services.AddAutoMapper(typeof(MappingDomainProfile), typeof(MappingControllerProfile));

                //Add Swagger
                services.AddSwaggerGen(x =>
                {
                    x.UseAllOfToExtendReferenceSchemas();
                    x.SupportNonNullableReferenceTypes();
                    x.AddEnumsWithValuesFixFilters();
                    x.SwaggerDoc("v1", new OpenApiInfo { Title = "AnagraficaAPI", Version = "v1" });
                    if (securitySettings.EnableIdentityServer)
                    {
                        x.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                        {
                            Type = SecuritySchemeType.OAuth2,
                            Flows = new OpenApiOAuthFlows
                            {
                                AuthorizationCode = new OpenApiOAuthFlow
                                {

                                    AuthorizationUrl = new Uri(securitySettings.AuthorityUrl + "/connect/authorize"),
                                    TokenUrl = new Uri(securitySettings.AuthorityUrl + "/connect/token"),
                                    Scopes = new Dictionary<string, string>
                                {
                                    {securitySettings.RequiredScope, "Anagrafica API - full access"}
                                }
                                },
                                ClientCredentials = new OpenApiOAuthFlow
                                {

                                    AuthorizationUrl = new Uri(securitySettings.AuthorityUrl + "/connect /authorize"),
                                    TokenUrl = new Uri(securitySettings.AuthorityUrl + "/connect/token"),
                                    Scopes = new Dictionary<string, string>
                                    {
                                        {securitySettings.RequiredScope, "Anagrafica API - full access"}
                                    }
                                },
                            }
                        });

                        x.OperationFilter<AuthorizeCheckOperationFilter>();
                    }
                });

                //DI Services Scopped
                services.AddScoped<ISystemInfoApiService, SystemInfoApiService>();
                services.AddScoped<IAnagraficaService, AnagraficaService>();
                services.AddScoped<ITagService, TagService>();
                services.AddScoped<IIndirizzoService, IndirizzoService>();
                services.AddScoped<IAnagraficaCustomFilter, AnagraficaCustomFilter>();
                services.AddScoped<IDocService>(a => { return new DocServiceClient(DocServiceClient.EndpointConfiguration.BasicHttpBinding_IDocService, appSettings.DocsGeneratorUrl); });
                services.AddScoped<IDocumentoService, DocumentoService>();
                services.AddScoped<IExportService, ExportService>();

                //DI Authorization Service Scoped
                services.AddScoped<IProfileService, ProfileSQLServerService>();
                services.AddScoped<IAuthorizationManager, AuthorizationManager>();
                services.AddScoped<AuthorizationManagerActionFilter>();

                services.AddMvc().AddMvcOptions(
                options =>
                {
                    options.Filters.AddService<AuthorizationManagerActionFilter>();
                });

                //Configure Kestrel
                services.Configure<KestrelServerOptions>(_configuration.GetSection("Kestrel"));

                // Configure Cors
                services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy-Olympus",
                        builder => builder
                            .WithOrigins(appSettings.CorsEnabledDomains != null ? appSettings.CorsEnabledDomains.ToArray() : Array.Empty<string>())
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .Build());
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<SecuritySettings> securitySettings, IOptions<AppSettings> appSettingsOptions)
        {
            Log.Information($"Startup::Configure env.EnvironmentName:{env.EnvironmentName}");

            try
            {
                app.UseZeusApiMiddleware();
                app.UseCors("CorsPolicy-Olympus");
                app.UseRouting();
                app.UseRequestLocalization();
                app.UseHttpsRedirection();
                if (securitySettings.Value.EnableIdentityServer)
                {
                    app.UseAuthentication();
                    app.UseAuthorization();
                }
                app.UseSwagger(c =>
                {
                    c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                    {
                        if (string.IsNullOrEmpty(appSettingsOptions.Value.OpenApiServer) == false)
                            swaggerDoc.Servers = new List<OpenApiServer>()
                            {
                                new OpenApiServer { Url = appSettingsOptions.Value.OpenApiServer, Description = "Api Gateway Server" }
                            };
                        else
                            swaggerDoc.Servers.Clear();
                    });
                });
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("../swagger/v1/swagger.json", "AnagraficaAPI");
                    options.DisplayOperationId();
                    options.DisplayRequestDuration();
                    if (securitySettings.Value.EnableIdentityServer)
                    {
                        options.OAuthClientId(securitySettings.Value.ClientId);
                        options.OAuthAppName("Anagrafica API - Swagger");
                        options.OAuthUsePkce();
                        options.OAuthClientSecret(securitySettings.Value.Secret);
                    }
                });

                app.UseEndpoints(endpoints =>
                {
                    var builder = endpoints.MapControllers();
                    if (securitySettings.Value.EnableIdentityServer)
                        builder.RequireAuthorization();
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }
    }
}
