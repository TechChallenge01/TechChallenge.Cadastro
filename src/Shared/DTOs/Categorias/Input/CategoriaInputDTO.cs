namespace Shared.DTOs.Categorias.Input;

public class CategoriaInputDTO
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public Guid IdUsuarioCriacao { get; set; }
    public DateTime DataCriacao { get; set; }
    public Guid? IdUsuarioAtualizacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
    public bool Ativo { get; set; }
}
