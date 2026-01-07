using MediatR;

namespace MonConnect.Application.Products.Commands
{
    public class CreateProductCommand : IRequest<Guid>
    {
        public string Nombre { get; set; } = null!;
        public string Codigo { get; set; } = null!;
        public string Categoria { get; set; } = null!;
        public string Material { get; set; } = null!;
        public string Acabado { get; set; } = null!;
        public string Color { get; set; } = null!;

        public decimal PrecioPorMetroCuadrado { get; set; }
        public decimal PrecioPorCaja { get; set; }
        public decimal MetrosCuadradosPorCaja { get; set; }
    }
}
