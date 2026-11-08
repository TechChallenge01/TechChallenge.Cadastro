using Application.Interfaces;
using Domain.Aggregates;
using Shared.DTOs.Produtos.Input;

namespace Application.Gateways.Produtos;

public class ProdutoGateway
{
    private readonly IProdutoDataSource _dataSource;

    private ProdutoGateway(IProdutoDataSource dataSource) => _dataSource = dataSource;

    public static ProdutoGateway Create(IProdutoDataSource dataSource) => new(dataSource);

    public async Task<Produto?> GetById(Guid id, CancellationToken ct)
    {
        var dto = await _dataSource.GetById(id, ct);
        return dto is null ? null : Reconstituir(dto);
    }

    public async Task<(List<Produto>, int)> GetPaginated(int page, int pageSize, CancellationToken ct)
    {
        var (dtos, total) = await _dataSource.GetPaginated(page, pageSize, ct);
        return (dtos.Select(Reconstituir).ToList(), total);
    }

    public async Task Create(Produto produto, CancellationToken ct)
        => await _dataSource.Create(Serializar(produto), ct);

    public async Task Update(Produto produto, CancellationToken ct)
        => await _dataSource.Update(Serializar(produto), ct);

    private static Produto Reconstituir(ProdutoInputDTO dto) =>
        new(dto.Id, dto.Nome, dto.Descricao, dto.Preco, dto.CategoriaId,
            dto.IdUsuarioCriacao, dto.DataCriacao,
            dto.IdUsuarioAtualizacao, dto.DataAtualizacao, dto.Ativo);

    private static ProdutoInputDTO Serializar(Produto p) => new()
    {
        Id = p.Id,
        Nome = p.Nome,
        Descricao = p.Descricao,
        Preco = p.Preco,
        CategoriaId = p.CategoriaId,
        IdUsuarioCriacao = p.IdUsuarioCriacao,
        DataCriacao = p.DataCriacao,
        IdUsuarioAtualizacao = p.IdUsuarioAtualizacao,
        DataAtualizacao = p.DataAtualizacao,
        Ativo = p.Ativo
    };
}
