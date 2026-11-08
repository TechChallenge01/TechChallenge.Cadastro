using API.Extensions;
using Application.Controllers.Cadastro;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Categorias.Request;

namespace API.EndPoints.Categorias;

public class CriarCategoria : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/categorias", async (
            [FromBody] CriarCategoriaRequestDTO request,
            HttpContext ctx,
            IProdutoDataSource produtoDataSource,
            ICategoriaDataSource categoriaDataSource,
            IClienteDataSource clienteDataSource,
            CancellationToken ct) =>
        {
            var idUsuario = ctx.User.GetUserId();
            var controller = new CadastroController(produtoDataSource, categoriaDataSource, clienteDataSource);
            var result = await controller.CriarCategoria(request, idUsuario, ct);
            return result.ToMinimalApiResult();
        })
        .RequireAuthorization()
        .WithTags("Categorias")
        .WithSummary("Cria uma nova categoria");
    }
}
