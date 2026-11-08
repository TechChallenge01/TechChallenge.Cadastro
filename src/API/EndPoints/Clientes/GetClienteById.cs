using API.Extensions;
using Application.Controllers.Cadastro;
using Application.Interfaces;

namespace API.EndPoints.Clientes;

public class GetClienteById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/clientes/{id:guid}", async (
            Guid id,
            IProdutoDataSource produtoDataSource,
            ICategoriaDataSource categoriaDataSource,
            IClienteDataSource clienteDataSource,
            CancellationToken ct) =>
        {
            var controller = new CadastroController(produtoDataSource, categoriaDataSource, clienteDataSource);
            var result = await controller.GetClienteById(id, ct);
            return result.ToMinimalApiResult();
        })
        .RequireAuthorization()
        .WithTags("Clientes")
        .WithSummary("Busca cliente por ID");
    }
}
