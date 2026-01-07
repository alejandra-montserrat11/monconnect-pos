
using MediatR;
using MonConnect.Application.Dashboard.DTOs;

public record GetDashboardPosQuery(Guid SucursalId) : IRequest<DashboardPosDto>;