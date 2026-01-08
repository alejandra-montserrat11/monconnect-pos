
using Microsoft.AspNetCore.Authorization;
using MonConnect.Domain.Constants; 

public static class Policies
{
    public const string PuedeExportarReportes = "PuedesExportarReportes";

    public static AuthorizationPolicy ExportarReportesPolicy()
       => new AuthorizationPolicyBuilder()
           .RequireRole(Roles.Admin)
           .Build();

}