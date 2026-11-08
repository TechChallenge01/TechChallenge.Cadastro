using Domain.Aggregates;
using FluentAssertions;

namespace UnitTests.Domain;

public class CategoriaTests
{
    [Fact]
    public void CriarCategoria_DeveInicializarPropriedades()
    {
        var usuario = Guid.NewGuid();

        var categoria = new Categoria("Peças de Motor", usuario);

        categoria.Nome.Should().Be("Peças de Motor");
        categoria.IdUsuarioCriacao.Should().Be(usuario);
    }

    [Fact]
    public void CriarCategoria_DeveGerarId()
    {
        var categoria = new Categoria("Lubrificantes", Guid.NewGuid());

        categoria.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void CriarCategoria_DeveEstarAtiva()
    {
        var categoria = new Categoria("Filtros", Guid.NewGuid());

        categoria.Ativo.Should().BeTrue();
    }

    [Fact]
    public void Inativar_DeveMudarAtivoParaFalso()
    {
        var categoria = new Categoria("Pneus", Guid.NewGuid());

        categoria.Inativar();

        categoria.Ativo.Should().BeFalse();
    }

    [Fact]
    public void CriarCategoria_DeveRegistrarDataCriacao()
    {
        var antes = DateTime.UtcNow;

        var categoria = new Categoria("Freios", Guid.NewGuid());

        categoria.DataCriacao.Should().BeOnOrAfter(antes);
    }
}
