using Application.Gateways.Categorias;
using Application.Presenters.Categorias;
using Domain.Aggregates;
using Shared.DTOs.Categorias.Request;
using Shared.Result;

namespace Application.UseCases.Categorias;

public class CriarCategoriaUseCase
{
    private readonly CategoriaGateway _gateway;
    private readonly CategoriaPresenter _presenter = new("Categoria criada com sucesso.");

    private CriarCategoriaUseCase(CategoriaGateway gateway) => _gateway = gateway;

    public static CriarCategoriaUseCase Create(CategoriaGateway gateway) => new(gateway);

    public async Task<ICommandResult> Run(CriarCategoriaRequestDTO request, Guid idUsuario, CancellationToken ct)
    {
        var categoria = new Categoria(request.Nome, idUsuario);
        await _gateway.Create(categoria, ct);
        return _presenter.Created(categoria.Id);
    }
}
