
using MediatR;
using MonConnect.Domain.Entities;
using System.Collections.Generic;

namespace MonConnect.Application.Products.Queries
{
    public class GetProductosQuery : IRequest<List<Producto>>
    {
        
    }
}