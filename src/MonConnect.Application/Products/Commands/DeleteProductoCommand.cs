

using MediatR;

public record DeleteProductoCommand(Guid Id) : IRequest<string>;
