using Domain.BaseEntity;

namespace Domain.Aggregates;

public class Categoria : Base
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;

    public Categoria(string nome, Guid idUsuario)
        : base(idUsuario, DateTime.UtcNow)
    {
        Id = Guid.NewGuid();
        Nome = nome;
    }

    public Categoria(Guid id, string nome,
        Guid idUsuarioCriacao, DateTime dataCriacao,
        Guid? idUsuarioAtualizacao, DateTime? dataAtualizacao, bool ativo)
        : base(idUsuarioCriacao, dataCriacao, idUsuarioAtualizacao, dataAtualizacao)
    {
        Id = id;
        Nome = nome;
        Ativo = ativo;
    }
}
