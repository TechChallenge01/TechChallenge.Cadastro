using API.Extensions;
using Application.Controllers.Cadastro;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Clientes.Request;

namespace API.EndPoints.Clientes;

public class CadastrarCliente : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/clientes", async (
            [FromBody] CadastrarClienteRequestDTO request,
            HttpContext ctx,
            IProdutoDataSource produtoDataSource,
            ICategoriaDataSource categoriaDataSource,
            IClienteDataSource clienteDataSource,
            IEventPublisher eventPublisher,
            CancellationToken ct) =>
        {
            var idUsuario = ctx.User.GetUserId();
            var controller = new CadastroController(produtoDataSource, categoriaDataSource, clienteDataSource);
            var result = await controller.CadastrarCliente(request, idUsuario, eventPublisher, ct);
            return result.ToMinimalApiResult();
        })
        .RequireAuthorization()
        .WithTags("Clientes")
        .WithSummary("Cadastra um novo cliente");
    }
}
