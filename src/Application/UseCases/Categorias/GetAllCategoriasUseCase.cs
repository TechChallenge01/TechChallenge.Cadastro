using Application.Gateways.Categorias;
using Application.Presenters.Categorias;
using Shared.Result;

namespace Application.UseCases.Categorias;

public class GetAllCategoriasUseCase
{
    private readonly CategoriaGateway _gateway;
    private readonly CategoriaPresenter _presenter = new("Categorias encontradas.");

    private GetAllCategoriasUseCase(CategoriaGateway gateway) => _gateway = gateway;

    public static GetAllCategoriasUseCase Create(CategoriaGateway gateway) => new(gateway);

    public async Task<ICommandResult> Run(CancellationToken ct)
    {
        var categorias = await _gateway.GetAll(ct);
        return _presenter.TransformList(categorias);
    }
}
