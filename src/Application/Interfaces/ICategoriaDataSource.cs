using Shared.DTOs.Categorias.Input;

namespace Application.Interfaces;

public interface ICategoriaDataSource
{
    Task<CategoriaInputDTO?> GetById(Guid id, CancellationToken ct);
    Task<List<CategoriaInputDTO>> GetAll(CancellationToken ct);
    Task Create(CategoriaInputDTO dto, CancellationToken ct);
}
