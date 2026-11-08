namespace Shared.DTOs.Categorias.Output;

public class CategoriaOutputDTO
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
}
