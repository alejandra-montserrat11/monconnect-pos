
using Microsoft.Extensions.DependencyInjection;
using MonConnect.Application.Common.Interfaces;
//using MonConnect.Infrastructure.Services;

namespace MonConnect.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IExcelExportService, ExcelExportService>();

        return services;
    }
}
