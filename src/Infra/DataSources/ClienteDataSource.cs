using Application.Interfaces;
using MongoDB.Driver;
using Shared.DTOs.Clientes.Input;

namespace Infra.DataSources;

public class ClienteDataSource : IClienteDataSource
{
    private readonly IMongoCollection<ClienteInputDTO> _collection;

    public ClienteDataSource(IMongoDatabase database)
        => _collection = database.GetCollection<ClienteInputDTO>("clientes");

    public async Task<ClienteInputDTO?> GetById(Guid id, CancellationToken ct)
        => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(ct);

    public async Task<ClienteInputDTO?> GetByEmail(string email, CancellationToken ct)
        => await _collection.Find(x => x.Email == email).FirstOrDefaultAsync(ct);

    public async Task<ClienteInputDTO?> GetByCpf(string cpf, CancellationToken ct)
        => await _collection.Find(x => x.Cpf == cpf).FirstOrDefaultAsync(ct);

    public async Task Create(ClienteInputDTO dto, CancellationToken ct)
        => await _collection.InsertOneAsync(dto, cancellationToken: ct);
}
