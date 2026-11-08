using System.Net;
using Domain.Aggregates;
using Shared.DTOs.Categorias.Output;
using Shared.Result;

namespace Application.Presenters.Categorias;

public class CategoriaPresenter
{
    private readonly string _message;

    public CategoriaPresenter(string message) => _message = message;

    public ICommandResult<CategoriaOutputDTO> TransformObject(Categoria categoria)
        => new CommandResult<CategoriaOutputDTO>(_message, HttpStatusCode.OK, Transform(categoria));

    public ICommandResult<List<CategoriaOutputDTO>> TransformList(List<Categoria> categorias)
        => new CommandResult<List<CategoriaOutputDTO>>(_message, HttpStatusCode.OK, categorias.Select(Transform).ToList());

    public ICommandResult<T> Created<T>(T data)
        => new CommandResult<T>(_message, HttpStatusCode.Created, data);

    public ICommandResult NotFound(string? message = null)
        => new CommandResult(message ?? _message, HttpStatusCode.NotFound);

    private static CategoriaOutputDTO Transform(Categoria c) => new()
    {
        Id = c.Id,
        Nome = c.Nome,
        Ativo = c.Ativo,
        DataCriacao = c.DataCriacao
    };
}
