
using MediatR;
using Microsoft.EntityFrameworkCore;
using MonConnect.Application.Common.Interfaces;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        var producto = await _context.Productos
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if(producto == null)
          throw new Exception ("Producto no encontrado");

        if(!producto.Activo)
          throw new Exception("No se puede editar un producto desactivado");


        producto.Nombre = request.Nombre;
        producto.Codigo = request.Codigo;
        producto.Categoria = request.Categoria;
        producto.Material = request.Material;
        producto.Acabado = request.Acabado;
        producto.Color = request.Color;
        producto.PrecioPorMetroCuadrado = request.PrecioPorMetroCuadrado;
        producto.PrecioPorCaja = request.PrecioPorCaja;
        producto.MetrosCuadradosPorCaja = request.MetrosCuadradosPorCaja;

        await _context.SaveChangesAsync(cancellationToken);
    }
}