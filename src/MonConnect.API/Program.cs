using Microsoft.EntityFrameworkCore;
using MediatR;
using MonConnect.Application.Products.Commands;
using MonConnect.Application.Common.Interfaces;
using MonConnect.Infrastructure.Persistence;
using MonConnect.Infrastructure;
using MonConnect.Application.Ventas;
using MonConnect.Application.Common.Behaviors;
using QuestPDF.Infrastructure;
using FluentValidation;

// NUEVOS USINGS PARA JWT
//using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
//using MonConnect.Application.Common.Models; 
using MonConnect.Domain.Constants;
using Microsoft.OpenApi.Models;
using MonConnect.API.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using MonConnect.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. CONFIGURACIÓN JWT (Extraer datos de appsettings.json)
var jwtSettings = new JwtSettings();
builder.Configuration.GetSection("JwtSettings").Bind(jwtSettings);
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Registrar el servicio de Tokens JWT
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// Controllers
builder.Services.AddControllers();

// Swagger con configuración de Seguridad (Botón "Authorize")
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MonConnect API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando el esquema Bearer. Ejemplo: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});

// DbContext
builder.Services.AddDbContext<MonConnectDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Clean Architecture bridge
builder.Services.AddScoped<IApplicationDbContext>(
    provider => provider.GetRequiredService<MonConnectDbContext>());

// 2. CONFIGURAR AUTENTICACIÓN
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings.Secret))
    };
});

//Registrar current service
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddHttpContextAccessor();

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly));

//REGISTRA INFRASTRUCTURE
builder.Services.AddInfrastructure();

QuestPDF.Settings.License = LicenseType.Community;

//Authorization & Policies
builder.Services.AddAuthorization(options =>
{
    // Registramos la política usando los métodos de tu clase Policies
    options.AddPolicy(
        Policies.PuedeExportarReportes, 
        Policies.ExportarReportesPolicy()
    );
});

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateVentaCommandValidator>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

var app = builder.Build();

// Middleware (EL ORDEN ES VITAL AQUÍ)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 3. AGREGAR ESTOS DOS EN ESTE ORDEN EXACTO
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();