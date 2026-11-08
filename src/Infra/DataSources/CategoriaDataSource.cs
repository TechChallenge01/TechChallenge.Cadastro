using Application.Interfaces;
using MongoDB.Driver;
using Shared.DTOs.Categorias.Input;

namespace Infra.DataSources;

public class CategoriaDataSource : ICategoriaDataSource
{
    private readonly IMongoCollection<CategoriaInputDTO> _collection;

    public CategoriaDataSource(IMongoDatabase database)
        => _collection = database.GetCollection<CategoriaInputDTO>("categorias");

    public async Task<CategoriaInputDTO?> GetById(Guid id, CancellationToken ct)
        => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(ct);

    public async Task<List<CategoriaInputDTO>> GetAll(CancellationToken ct)
        => await _collection.Find(x => x.Ativo).ToListAsync(ct);

    public async Task Create(CategoriaInputDTO dto, CancellationToken ct)
        => await _collection.InsertOneAsync(dto, cancellationToken: ct);
}
