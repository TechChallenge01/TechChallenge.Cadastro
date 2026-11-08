using Application.Interfaces;
using Domain.Aggregates;
using Shared.DTOs.Categorias.Input;

namespace Application.Gateways.Categorias;

public class CategoriaGateway
{
    private readonly ICategoriaDataSource _dataSource;

    private CategoriaGateway(ICategoriaDataSource dataSource) => _dataSource = dataSource;

    public static CategoriaGateway Create(ICategoriaDataSource dataSource) => new(dataSource);

    public async Task<Categoria?> GetById(Guid id, CancellationToken ct)
    {
        var dto = await _dataSource.GetById(id, ct);
        return dto is null ? null : Reconstituir(dto);
    }

    public async Task<List<Categoria>> GetAll(CancellationToken ct)
    {
        var dtos = await _dataSource.GetAll(ct);
        return dtos.Select(Reconstituir).ToList();
    }

    public async Task Create(Categoria categoria, CancellationToken ct)
        => await _dataSource.Create(Serializar(categoria), ct);

    private static Categoria Reconstituir(CategoriaInputDTO dto) =>
        new(dto.Id, dto.Nome,
            dto.IdUsuarioCriacao, dto.DataCriacao,
            dto.IdUsuarioAtualizacao, dto.DataAtualizacao, dto.Ativo);

    private static CategoriaInputDTO Serializar(Categoria c) => new()
    {
        Id = c.Id,
        Nome = c.Nome,
        IdUsuarioCriacao = c.IdUsuarioCriacao,
        DataCriacao = c.DataCriacao,
        IdUsuarioAtualizacao = c.IdUsuarioAtualizacao,
        DataAtualizacao = c.DataAtualizacao,
        Ativo = c.Ativo
    };
}
