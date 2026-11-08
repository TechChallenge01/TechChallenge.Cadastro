using Domain.BaseEntity;

namespace Domain.Aggregates;

public class Cliente : Base
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Cpf { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;

    public Cliente(string nome, string cpf, string email, Guid idUsuario)
        : base(idUsuario, DateTime.UtcNow)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Cpf = cpf;
        Email = email;
    }

    public Cliente(Guid id, string nome, string cpf, string email,
        Guid idUsuarioCriacao, DateTime dataCriacao,
        Guid? idUsuarioAtualizacao, DateTime? dataAtualizacao, bool ativo)
        : base(idUsuarioCriacao, dataCriacao, idUsuarioAtualizacao, dataAtualizacao)
    {
        Id = id;
        Nome = nome;
        Cpf = cpf;
        Email = email;
        Ativo = ativo;
    }
}
