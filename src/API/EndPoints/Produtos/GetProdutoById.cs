using API.Extensions;
using Application.Controllers.Cadastro;
using Application.Interfaces;

namespace API.EndPoints.Produtos;

public class GetProdutoById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/produtos/{id:guid}", async (
            Guid id,
            IProdutoDataSource produtoDataSource,
            ICategoriaDataSource categoriaDataSource,
            IClienteDataSource clienteDataSource,
            CancellationToken ct) =>
        {
            var controller = new CadastroController(produtoDataSource, categoriaDataSource, clienteDataSource);
            var result = await controller.GetProdutoById(id, ct);
            return result.ToMinimalApiResult();
        })
        .RequireAuthorization()
        .WithTags("Produtos")
        .WithSummary("Busca produto por ID");
    }
}
