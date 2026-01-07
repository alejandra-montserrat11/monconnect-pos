
using MediatR;
using MonConnect.Application.Inventarios.DTOs;

public class GetAlertasStockBajoQuery : IRequest<List<AlertaStockDto>>
{
    public Guid? SucursalId {get; set;} //opcional ya que puede ser por sucursal o global
}