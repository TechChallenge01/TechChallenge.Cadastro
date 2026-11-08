using Application.Interfaces;
using MongoDB.Driver;
using Shared.DTOs.Produtos.Input;

namespace Infra.DataSources;

public class ProdutoDataSource : IProdutoDataSource
{
    private readonly IMongoCollection<ProdutoInputDTO> _collection;

    public ProdutoDataSource(IMongoDatabase database)
        => _collection = database.GetCollection<ProdutoInputDTO>("produtos");

    public async Task<ProdutoInputDTO?> GetById(Guid id, CancellationToken ct)
        => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(ct);

    public async Task<(List<ProdutoInputDTO>, int)> GetPaginated(int page, int pageSize, CancellationToken ct)
    {
        var filter = Builders<ProdutoInputDTO>.Filter.Empty;
        var total = (int)await _collection.CountDocumentsAsync(filter, cancellationToken: ct);
        var items = await _collection.Find(filter)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(ct);
        return (items, total);
    }

    public async Task Create(ProdutoInputDTO dto, CancellationToken ct)
        => await _collection.InsertOneAsync(dto, cancellationToken: ct);

    public async Task Update(ProdutoInputDTO dto, CancellationToken ct)
        => await _collection.ReplaceOneAsync(x => x.Id == dto.Id, dto, cancellationToken: ct);
}
