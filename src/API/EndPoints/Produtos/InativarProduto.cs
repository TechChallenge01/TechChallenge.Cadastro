using API.Extensions;
using Application.Controllers.Cadastro;
using Application.Interfaces;

namespace API.EndPoints.Produtos;

public class InativarProduto : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/produtos/{id:guid}", async (
            Guid id,
            HttpContext ctx,
            IProdutoDataSource produtoDataSource,
            ICategoriaDataSource categoriaDataSource,
            IClienteDataSource clienteDataSource,
            IEventPublisher eventPublisher,
            CancellationToken ct) =>
        {
            var idUsuario = ctx.User.GetUserId();
            var controller = new CadastroController(produtoDataSource, categoriaDataSource, clienteDataSource);
            var result = await controller.InativarProduto(id, idUsuario, eventPublisher, ct);
            return result.ToMinimalApiResult();
        })
        .RequireAuthorization()
        .WithTags("Produtos")
        .WithSummary("Inativa um produto");
    }
}
