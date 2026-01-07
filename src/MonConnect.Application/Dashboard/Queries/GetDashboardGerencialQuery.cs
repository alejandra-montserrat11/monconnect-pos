

using MediatR;
using MonConnect.Application.Dashboard.DTOs;

public class GetDashboardGerencialQuery : IRequest<DashboardGerencialDto>
{
    public Guid SucursalId { get; set; }
}
