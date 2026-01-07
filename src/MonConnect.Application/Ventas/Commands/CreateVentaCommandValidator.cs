using FluentValidation;
using MonConnect.Application.Ventas.Commands; // Asegúrate de que apunte a donde está tu Command y DTO

public class CreateVentaCommandValidator : AbstractValidator<CreateVentaCommand>
{
    public CreateVentaCommandValidator()
    {
        RuleFor(x => x.SucursalId)
            .NotEmpty()
            .WithMessage("Sucursal inválida");

        // Validamos que la lista de productos no venga vacía
        RuleFor(x => x.Productos)
            .NotEmpty()
            .WithMessage("La venta debe tener al menos un producto");

        RuleForEach<CreateVentaDetalleDto>(x => x.Productos).ChildRules(detalle =>
        {
            detalle.RuleFor(d => d.ProductoId)
                .NotEmpty()
                .WithMessage("Producto inválido");

            detalle.RuleFor(d => d.Cantidad)
                .GreaterThan(0)
                .WithMessage("La cantidad debe ser mayor a cero");
        });
    }
}