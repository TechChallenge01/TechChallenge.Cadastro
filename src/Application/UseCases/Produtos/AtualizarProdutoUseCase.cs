using Application.Gateways.Produtos;
using Application.Interfaces;
using Application.Presenters.Produtos;
using Shared.DTOs.Produtos.Request;
using Shared.Result;

namespace Application.UseCases.Produtos;

public class AtualizarProdutoUseCase
{
    private readonly ProdutoGateway _gateway;
    private readonly IEventPublisher _eventPublisher;
    private readonly ProdutoPresenter _presenter = new("Produto atualizado com sucesso.");

    private AtualizarProdutoUseCase(ProdutoGateway gateway, IEventPublisher eventPublisher)
    {
        _gateway = gateway;
        _eventPublisher = eventPublisher;
    }

    public static AtualizarProdutoUseCase Create(ProdutoGateway gateway, IEventPublisher eventPublisher)
        => new(gateway, eventPublisher);

    public async Task<ICommandResult> Run(Guid id, AtualizarProdutoRequestDTO request, Guid idUsuario, CancellationToken ct)
    {
        var produto = await _gateway.GetById(id, ct);
        if (produto is null)
            return _presenter.NotFound("Produto não encontrado.");

        produto.Atualizar(request.Nome, request.Descricao, request.Preco, request.CategoriaId, idUsuario);

        await _gateway.Update(produto, ct);

        await _eventPublisher.PublishAsync("cadastro.produto.atualizado", new
        {
            ProdutoId = produto.Id,
            produto.Nome,
            produto.Preco,
            produto.CategoriaId
        }, ct);

        return _presenter.TransformObject(produto);
    }
}
