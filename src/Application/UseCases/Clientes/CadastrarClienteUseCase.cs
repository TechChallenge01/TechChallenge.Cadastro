using Application.Gateways.Clientes;
using Application.Interfaces;
using Application.Presenters.Clientes;
using Domain.Aggregates;
using Shared.DTOs.Clientes.Request;
using Shared.Result;

namespace Application.UseCases.Clientes;

public class CadastrarClienteUseCase
{
    private readonly ClienteGateway _gateway;
    private readonly IEventPublisher _eventPublisher;
    private readonly ClientePresenter _presenter = new("Cliente cadastrado com sucesso.");

    private CadastrarClienteUseCase(ClienteGateway gateway, IEventPublisher eventPublisher)
    {
        _gateway = gateway;
        _eventPublisher = eventPublisher;
    }

    public static CadastrarClienteUseCase Create(ClienteGateway gateway, IEventPublisher eventPublisher)
        => new(gateway, eventPublisher);

    public async Task<ICommandResult> Run(CadastrarClienteRequestDTO request, Guid idUsuario, CancellationToken ct)
    {
        var existente = await _gateway.GetByEmail(request.Email, ct);
        if (existente is not null)
            return _presenter.BadRequest("Já existe um cliente cadastrado com esse e-mail.");

        var cliente = new Cliente(request.Nome, request.Cpf, request.Email, idUsuario);
        await _gateway.Create(cliente, ct);

        await _eventPublisher.PublishAsync("cadastro.cliente.cadastrado", new
        {
            ClienteId = cliente.Id,
            cliente.Nome,
            cliente.Cpf,
            cliente.Email
        }, ct);

        return _presenter.Created(cliente.Id);
    }
}
