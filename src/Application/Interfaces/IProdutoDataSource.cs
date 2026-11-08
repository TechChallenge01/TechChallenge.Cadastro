using Shared.DTOs.Produtos.Input;

namespace Application.Interfaces;

public interface IProdutoDataSource
{
    Task<ProdutoInputDTO?> GetById(Guid id, CancellationToken ct);
    Task<(List<ProdutoInputDTO>, int)> GetPaginated(int page, int pageSize, CancellationToken ct);
    Task Create(ProdutoInputDTO dto, CancellationToken ct);
    Task Update(ProdutoInputDTO dto, CancellationToken ct);
}
