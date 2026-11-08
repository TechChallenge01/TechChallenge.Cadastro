using API.Extensions;
using Application.Controllers.Cadastro;
using Application.Interfaces;

namespace API.EndPoints.Categorias;

public class GetAllCategorias : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/categorias", async (
            IProdutoDataSource produtoDataSource,
            ICategoriaDataSource categoriaDataSource,
            IClienteDataSource clienteDataSource,
            CancellationToken ct) =>
        {
            var controller = new CadastroController(produtoDataSource, categoriaDataSource, clienteDataSource);
            var result = await controller.GetAllCategorias(ct);
            return result.ToMinimalApiResult();
        })
        .RequireAuthorization()
        .WithTags("Categorias")
        .WithSummary("Lista todas as categorias ativas");
    }
}
