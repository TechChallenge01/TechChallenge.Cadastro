using API.Extensions;
using Application.Controllers.Cadastro;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Produtos.Request;

namespace API.EndPoints.Produtos;

public class CriarProduto : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/produtos", async (
            [FromBody] CriarProdutoRequestDTO request,
            HttpContext ctx,
            IProdutoDataSource produtoDataSource,
            ICategoriaDataSource categoriaDataSource,
            IClienteDataSource clienteDataSource,
            IEventPublisher eventPublisher,
            IMetricsService metrics,
            CancellationToken ct) =>
        {
            var idUsuario = ctx.User.GetUserId();
            var controller = new CadastroController(produtoDataSource, categoriaDataSource, clienteDataSource);
            var result = await controller.CriarProduto(request, idUsuario, eventPublisher, ct);
            return result.ToMinimalApiResult();
        })
        .RequireAuthorization()
        .WithTags("Produtos")
        .WithSummary("Cria um novo produto");
    }
}
