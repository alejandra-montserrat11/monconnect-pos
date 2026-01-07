using MediatR;
using MonConnect.Application.Common.Interfaces;
using MonConnect.Domain.Entities;

namespace MonConnect.Application.Products.Commands
{
    public class CreateProductoCommandHandler
        : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateProductoCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(
            CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            var producto = new Producto
            {
                Nombre = request.Nombre,
                Codigo = request.Codigo,
                Categoria = request.Categoria,
                Material = request.Material,
                Acabado = request.Acabado,
                Color = request.Color,
                PrecioPorMetroCuadrado = request.PrecioPorMetroCuadrado,
                PrecioPorCaja = request.PrecioPorCaja,
                MetrosCuadradosPorCaja = request.MetrosCuadradosPorCaja
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync(cancellationToken);

            return producto.Id;
        }
    }
}
