using System.Net;
using Domain.Aggregates;
using Shared.DTOs.Clientes.Output;
using Shared.Result;

namespace Application.Presenters.Clientes;

public class ClientePresenter
{
    private readonly string _message;

    public ClientePresenter(string message) => _message = message;

    public ICommandResult<ClienteOutputDTO> TransformObject(Cliente cliente)
        => new CommandResult<ClienteOutputDTO>(_message, HttpStatusCode.OK, Transform(cliente));

    public ICommandResult<T> Created<T>(T data)
        => new CommandResult<T>(_message, HttpStatusCode.Created, data);

    public ICommandResult NotFound(string? message = null)
        => new CommandResult(message ?? _message, HttpStatusCode.NotFound);

    public ICommandResult BadRequest(string? message = null)
        => new CommandResult(message ?? _message, HttpStatusCode.BadRequest);

    private static ClienteOutputDTO Transform(Cliente c) => new()
    {
        Id = c.Id,
        Nome = c.Nome,
        Cpf = c.Cpf,
        Email = c.Email,
        Ativo = c.Ativo,
        DataCriacao = c.DataCriacao,
        DataAtualizacao = c.DataAtualizacao
    };
}
