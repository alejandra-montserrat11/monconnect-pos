
using MediatR;
using Microsoft.EntityFrameworkCore;
using MonConnect.Application.Common.Interfaces;
using MonConnect.Domain.Entities;

namespace MonConnect.Application.Products.Queries
{
    public class GetProductosQueryHandler: IRequestHandler<GetProductosQuery, List<Producto>>
    {
        private readonly IApplicationDbContext _context;
         
        public GetProductosQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Producto>> Handle(
            GetProductosQuery request,
            CancellationToken cancellationToken)
        {
            return await  _context.Productos
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        
    }
}