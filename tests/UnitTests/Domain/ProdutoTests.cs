using Domain.Aggregates;
using FluentAssertions;

namespace UnitTests.Domain;

public class ProdutoTests
{
    private static Produto CriarProduto(string nome = "Filtro de Óleo", string descricao = "Filtro premium", decimal preco = 45.90m)
        => new(nome, descricao, preco, Guid.NewGuid(), Guid.NewGuid());

    [Fact]
    public void CriarProduto_DeveInicializarPropriedades()
    {
        var categoriaId = Guid.NewGuid();
        var usuario = Guid.NewGuid();

        var produto = new Produto("Vela de Ignição", "Vela NGK", 25m, categoriaId, usuario);

        produto.Nome.Should().Be("Vela de Ignição");
        produto.Descricao.Should().Be("Vela NGK");
        produto.Preco.Should().Be(25m);
        produto.CategoriaId.Should().Be(categoriaId);
        produto.IdUsuarioCriacao.Should().Be(usuario);
    }

    [Fact]
    public void CriarProduto_DeveEstarAtivo()
    {
        var produto = CriarProduto();

        produto.Ativo.Should().BeTrue();
    }

    [Fact]
    public void CriarProduto_DeveGerarId()
    {
        var produto = CriarProduto();

        produto.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void Atualizar_DeveModificarCampos()
    {
        var produto = CriarProduto();
        var novaCategoria = Guid.NewGuid();
        var usuario = Guid.NewGuid();

        produto.Atualizar("Vela NGK Iridium", "Alta performance", 89.90m, novaCategoria, usuario);

        produto.Nome.Should().Be("Vela NGK Iridium");
        produto.Descricao.Should().Be("Alta performance");
        produto.Preco.Should().Be(89.90m);
        produto.CategoriaId.Should().Be(novaCategoria);
        produto.IdUsuarioAtualizacao.Should().Be(usuario);
    }

    [Fact]
    public void Inativar_DeveMudarAtivoParaFalso()
    {
        var produto = CriarProduto();

        produto.Inativar();

        produto.Ativo.Should().BeFalse();
    }
}
