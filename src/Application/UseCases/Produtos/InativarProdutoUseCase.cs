using Application.Gateways.Produtos;
using Application.Interfaces;
using Application.Presenters.Produtos;
using Shared.Result;

namespace Application.UseCases.Produtos;

public class InativarProdutoUseCase
{
    private readonly ProdutoGateway _gateway;
    private readonly IEventPublisher _eventPublisher;
    private readonly ProdutoPresenter _presenter = new("Produto inativado com sucesso.");

    private InativarProdutoUseCase(ProdutoGateway gateway, IEventPublisher eventPublisher)
    {
        _gateway = gateway;
        _eventPublisher = eventPublisher;
    }

    public static InativarProdutoUseCase Create(ProdutoGateway gateway, IEventPublisher eventPublisher)
        => new(gateway, eventPublisher);

    public async Task<ICommandResult> Run(Guid id, Guid idUsuario, CancellationToken ct)
    {
        var produto = await _gateway.GetById(id, ct);
        if (produto is null)
            return _presenter.NotFound("Produto não encontrado.");

        produto.Inativar();
        produto.RastrearAlteracao(idUsuario, DateTime.UtcNow);

        await _gateway.Update(produto, ct);

        await _eventPublisher.PublishAsync("cadastro.produto.inativado", new { ProdutoId = produto.Id }, ct);

        return _presenter.NoContent();
    }
}
