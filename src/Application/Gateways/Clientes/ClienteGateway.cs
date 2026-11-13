using Application.Interfaces;
using Domain.Aggregates;
using Shared.DTOs.Clientes.Input;

namespace Application.Gateways.Clientes;

public class ClienteGateway
{
    private readonly IClienteDataSource _dataSource;

    private ClienteGateway(IClienteDataSource dataSource) => _dataSource = dataSource;

    public static ClienteGateway Create(IClienteDataSource dataSource) => new(dataSource);

    public async Task<Cliente?> GetById(Guid id, CancellationToken ct)
    {
        var dto = await _dataSource.GetById(id, ct);
        return dto is null ? null : Reconstituir(dto);
    }

    public async Task<Cliente?> GetByEmail(string email, CancellationToken ct)
    {
        var dto = await _dataSource.GetByEmail(email, ct);
        return dto is null ? null : Reconstituir(dto);
    }

    public async Task<Cliente?> GetByCpf(string cpf, CancellationToken ct)
    {
        var dto = await _dataSource.GetByCpf(cpf, ct);
        return dto is null ? null : Reconstituir(dto);
    }

    public async Task Create(Cliente cliente, CancellationToken ct)
        => await _dataSource.Create(Serializar(cliente), ct);

    private static Cliente Reconstituir(ClienteInputDTO dto) =>
        new(dto.Id, dto.Nome, dto.Cpf, dto.Email,
            dto.IdUsuarioCriacao, dto.DataCriacao,
            dto.IdUsuarioAtualizacao, dto.DataAtualizacao, dto.Ativo);

    private static ClienteInputDTO Serializar(Cliente c) => new()
    {
        Id = c.Id,
        Nome = c.Nome,
        Cpf = c.Cpf,
        Email = c.Email,
        IdUsuarioCriacao = c.IdUsuarioCriacao,
        DataCriacao = c.DataCriacao,
        IdUsuarioAtualizacao = c.IdUsuarioAtualizacao,
        DataAtualizacao = c.DataAtualizacao,
        Ativo = c.Ativo
    };
}
