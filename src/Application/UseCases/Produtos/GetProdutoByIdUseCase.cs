using Application.Gateways.Produtos;
using Application.Presenters.Produtos;
using Shared.Result;

namespace Application.UseCases.Produtos;

public class GetProdutoByIdUseCase
{
    private readonly ProdutoGateway _gateway;
    private readonly ProdutoPresenter _presenter = new("Produto encontrado.");

    private GetProdutoByIdUseCase(ProdutoGateway gateway) => _gateway = gateway;

    public static GetProdutoByIdUseCase Create(ProdutoGateway gateway) => new(gateway);

    public async Task<ICommandResult> Run(Guid id, CancellationToken ct)
    {
        var produto = await _gateway.GetById(id, ct);
        return produto is null ? _presenter.NotFound("Produto não encontrado.") : _presenter.TransformObject(produto);
    }
}
