using API.Extensions;
using Application.Controllers.Cadastro;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Produtos.Request;

namespace API.EndPoints.Produtos;

public class AtualizarProduto : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/produtos/{id:guid}", async (
            Guid id,
            [FromBody] AtualizarProdutoRequestDTO request,
            HttpContext ctx,
            IProdutoDataSource produtoDataSource,
            ICategoriaDataSource categoriaDataSource,
            IClienteDataSource clienteDataSource,
            IEventPublisher eventPublisher,
            CancellationToken ct) =>
        {
            var idUsuario = ctx.User.GetUserId();
            var controller = new CadastroController(produtoDataSource, categoriaDataSource, clienteDataSource);
            var result = await controller.AtualizarProduto(id, request, idUsuario, eventPublisher, ct);
            return result.ToMinimalApiResult();
        })
        .RequireAuthorization()
        .WithTags("Produtos")
        .WithSummary("Atualiza um produto existente");
    }
}
