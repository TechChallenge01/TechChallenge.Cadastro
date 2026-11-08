using Application.Gateways.Produtos;
using Application.Presenters.Produtos;
using Shared.Result;

namespace Application.UseCases.Produtos;

public class GetProdutosPaginadosUseCase
{
    private readonly ProdutoGateway _gateway;
    private readonly ProdutoPresenter _presenter = new("Produtos encontrados.");

    private GetProdutosPaginadosUseCase(ProdutoGateway gateway) => _gateway = gateway;

    public static GetProdutosPaginadosUseCase Create(ProdutoGateway gateway) => new(gateway);

    public async Task<ICommandResult> Run(int page, int pageSize, CancellationToken ct)
    {
        var (items, total) = await _gateway.GetPaginated(page, pageSize, ct);
        return _presenter.TransformPaged(items, page, total, pageSize);
    }
}
