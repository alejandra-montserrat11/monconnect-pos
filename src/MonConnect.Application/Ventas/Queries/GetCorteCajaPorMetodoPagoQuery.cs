
using MediatR;

public record GetCorteCajaPorMetodoPagoQuery(
    Guid SucursalId,
    DateTime Fecha
) : IRequest<List<CorteCajaMetodoPagoDto>>;


