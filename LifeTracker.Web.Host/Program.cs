using Application;
using Application.Services;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using LifeTracker.Web.Core.Meiddlewares;
using LifeTracker.Web.Host;
using Microsoft.OpenApi.Models;
using Persistence.Sql;

IConfigurationRoot _appConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
_appConfiguration = (new ConfigurationAppService()).GetConfigurationRoot();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new PersistenceModule(false));
    builder.RegisterModule<DomainModule>();
});

builder.Services.AddEndpointsApiExplorer();
AuthConfigurer.Configure(builder.Services, _appConfiguration);
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware(typeof(ErrorHandlingMiddleWare));

app.MapControllers();

app.Run();