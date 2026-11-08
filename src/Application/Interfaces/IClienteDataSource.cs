using Shared.DTOs.Clientes.Input;

namespace Application.Interfaces;

public interface IClienteDataSource
{
    Task<ClienteInputDTO?> GetById(Guid id, CancellationToken ct);
    Task<ClienteInputDTO?> GetByEmail(string email, CancellationToken ct);
    Task Create(ClienteInputDTO dto, CancellationToken ct);
}
