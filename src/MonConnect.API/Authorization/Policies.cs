
using Microsoft.AspNetCore.Authorization;

public static class Policies
{
    public const string PuedeExportarReportes = "PuedesExportarReportes";

    public static AuthorizationPolicy ExportarReportesPolicy()
       => new AuthorizationPolicyBuilder()
           .RequireRole(Roles.Admin)
           .Build();

}