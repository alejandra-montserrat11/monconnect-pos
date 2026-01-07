using MediatR;
using Microsoft.AspNetCore.Mvc;
using MonConnect.Application.Products.Commands;
using MonConnect.Application.Products.Queries;

namespace MonConnect.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productos = await _mediator.Send(new GetProductosQuery());
            return Ok(productos);
        }

        
   //Actualizar productos
   //Actualizar informacion
   [HttpPut("{id}")]
   public async Task<ActionResult> Update(
    Guid id,
    UpdateProductCommand command)
    {
       command.Id = id;
       await _mediator.Send(command);
       return NoContent();
    }

    //Desactivar producto
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
    var result = await _mediator.Send(new DeleteProductoCommand(id));
    return Ok(result);
    }

    }
    
}
