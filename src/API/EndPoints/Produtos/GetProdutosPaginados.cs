using API.Extensions;
using Application.Controllers.Cadastro;
using Application.Interfaces;

namespace API.EndPoints.Produtos;

public class GetProdutosPaginados : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/produtos", async (
            int page,
            int pageSize,
            IProdutoDataSource produtoDataSource,
            ICategoriaDataSource categoriaDataSource,
            IClienteDataSource clienteDataSource,
            CancellationToken ct) =>
        {
            var controller = new CadastroController(produtoDataSource, categoriaDataSource, clienteDataSource);
            var result = await controller.GetProdutosPaginados(page, pageSize, ct);
            return result.ToMinimalApiResult();
        })
        .RequireAuthorization()
        .WithTags("Produtos")
        .WithSummary("Lista produtos paginados");
    }
}
