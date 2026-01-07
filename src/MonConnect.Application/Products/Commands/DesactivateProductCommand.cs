
using MediatR;
using System;

public class DesactivateProductCommand : IRequest
{
    public Guid Id {get; set;}
}