using Application.Gateways.Produtos;
using Application.Interfaces;
using Application.Presenters.Produtos;
using Domain.Aggregates;
using Shared.DTOs.Produtos.Request;
using Shared.Result;

namespace Application.UseCases.Produtos;

public class CriarProdutoUseCase
{
    private readonly ProdutoGateway _gateway;
    private readonly IEventPublisher _eventPublisher;
    private readonly ProdutoPresenter _presenter = new("Produto criado com sucesso.");

    private CriarProdutoUseCase(ProdutoGateway gateway, IEventPublisher eventPublisher)
    {
        _gateway = gateway;
        _eventPublisher = eventPublisher;
    }

    public static CriarProdutoUseCase Create(ProdutoGateway gateway, IEventPublisher eventPublisher)
        => new(gateway, eventPublisher);

    public async Task<ICommandResult> Run(CriarProdutoRequestDTO request, Guid idUsuario, CancellationToken ct)
    {
        var produto = new Produto(request.Nome, request.Descricao, request.Preco, request.CategoriaId, idUsuario);

        await _gateway.Create(produto, ct);

        await _eventPublisher.PublishAsync("cadastro.produto.criado", new
        {
            ProdutoId = produto.Id,
            produto.Nome,
            produto.Preco,
            produto.CategoriaId
        }, ct);

        return _presenter.Created(produto.Id);
    }
}
