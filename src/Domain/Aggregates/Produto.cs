using Domain.BaseEntity;

namespace Domain.Aggregates;

public class Produto : Base
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Descricao { get; private set; } = string.Empty;
    public decimal Preco { get; private set; }
    public Guid CategoriaId { get; private set; }

    public Produto(string nome, string descricao, decimal preco, Guid categoriaId, Guid idUsuario)
        : base(idUsuario, DateTime.UtcNow)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Descricao = descricao;
        Preco = preco;
        CategoriaId = categoriaId;
    }

    public Produto(Guid id, string nome, string descricao, decimal preco, Guid categoriaId,
        Guid idUsuarioCriacao, DateTime dataCriacao,
        Guid? idUsuarioAtualizacao, DateTime? dataAtualizacao, bool ativo)
        : base(idUsuarioCriacao, dataCriacao, idUsuarioAtualizacao, dataAtualizacao)
    {
        Id = id;
        Nome = nome;
        Descricao = descricao;
        Preco = preco;
        CategoriaId = categoriaId;
        Ativo = ativo;
    }

    public void Atualizar(string nome, string descricao, decimal preco, Guid categoriaId, Guid idUsuario)
    {
        Nome = nome;
        Descricao = descricao;
        Preco = preco;
        CategoriaId = categoriaId;
        RastrearAlteracao(idUsuario, DateTime.UtcNow);
    }
}
