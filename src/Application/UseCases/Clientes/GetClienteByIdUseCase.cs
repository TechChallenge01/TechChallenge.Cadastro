using Application.Gateways.Clientes;
using Application.Presenters.Clientes;
using Shared.Result;

namespace Application.UseCases.Clientes;

public class GetClienteByIdUseCase
{
    private readonly ClienteGateway _gateway;
    private readonly ClientePresenter _presenter = new("Cliente encontrado.");

    private GetClienteByIdUseCase(ClienteGateway gateway) => _gateway = gateway;

    public static GetClienteByIdUseCase Create(ClienteGateway gateway) => new(gateway);

    public async Task<ICommandResult> Run(Guid id, CancellationToken ct)
    {
        var cliente = await _gateway.GetById(id, ct);
        return cliente is null ? _presenter.NotFound("Cliente não encontrado.") : _presenter.TransformObject(cliente);
    }
}
