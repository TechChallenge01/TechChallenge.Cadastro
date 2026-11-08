using System.Net;
using Domain.Aggregates;
using Shared.DTOs;
using Shared.DTOs.Produtos.Output;
using Shared.Result;

namespace Application.Presenters.Produtos;

public class ProdutoPresenter
{
    private readonly string _message;

    public ProdutoPresenter(string message) => _message = message;

    public ICommandResult<ProdutoOutputDTO> TransformObject(Produto produto)
        => new CommandResult<ProdutoOutputDTO>(_message, HttpStatusCode.OK, Transform(produto));

    public ICommandResult<List<ProdutoOutputDTO>> TransformList(List<Produto> produtos)
        => new CommandResult<List<ProdutoOutputDTO>>(_message, HttpStatusCode.OK, produtos.Select(Transform).ToList());

    public ICommandResult<PagedResultDTO<ProdutoOutputDTO>> TransformPaged(List<Produto> items, int page, int total, int pageSize = 10)
        => new CommandResult<PagedResultDTO<ProdutoOutputDTO>>(_message, HttpStatusCode.PartialContent,
            new PagedResultDTO<ProdutoOutputDTO>
            {
                Items = items.Select(Transform).ToList(),
                Total = total,
                Page = page,
                PageSize = pageSize
            });

    public ICommandResult<T> Created<T>(T data)
        => new CommandResult<T>(_message, HttpStatusCode.Created, data);

    public ICommandResult NoContent()
        => new CommandResult(_message, HttpStatusCode.NoContent);

    public ICommandResult NotFound(string? message = null)
        => new CommandResult(message ?? _message, HttpStatusCode.NotFound);

    public ICommandResult BadRequest(string? message = null)
        => new CommandResult(message ?? _message, HttpStatusCode.BadRequest);

    private static ProdutoOutputDTO Transform(Produto p) => new()
    {
        Id = p.Id,
        Nome = p.Nome,
        Descricao = p.Descricao,
        Preco = p.Preco,
        CategoriaId = p.CategoriaId,
        Ativo = p.Ativo,
        DataCriacao = p.DataCriacao,
        DataAtualizacao = p.DataAtualizacao
    };
}
