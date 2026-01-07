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

var builder = WebApplication.CreateBuilder(args);

// 🔹 Controllers
builder.Services.AddControllers();

// 🔹 Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔹 DbContext
builder.Services.AddDbContext<MonConnectDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// 🔹 Clean Architecture bridge
builder.Services.AddScoped<IApplicationDbContext>(
    provider => provider.GetRequiredService<MonConnectDbContext>());

// 🔹 MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly));

// 👇 REGISTRA INFRASTRUCTURE
builder.Services.AddInfrastructure();

QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        Policies.PuedeExportarReportes,
        Policies.ExportarReportesPolicy()
    );
});

//fluent
// 1. Registra todos los validadores
builder.Services.AddValidatorsFromAssemblyContaining<CreateVentaCommandValidator>();

// 2. Registra el Behavior de MediatR
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


var app = builder.Build();

// 🔹 Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 🔹 ESTO HACE QUE APAREZCAN TUS CONTROLLERS
app.MapControllers();

app.Run();
