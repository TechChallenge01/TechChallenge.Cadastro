using Domain.Aggregates;
using FluentAssertions;

namespace UnitTests.Domain;

public class ClienteTests
{
    private static Cliente CriarCliente(string nome = "João Silva", string cpf = "12345678901", string email = "joao@example.com")
        => new(nome, cpf, email, Guid.NewGuid());

    [Fact]
    public void CriarCliente_DeveInicializarPropriedades()
    {
        var usuario = Guid.NewGuid();

        var cliente = new Cliente("Maria Santos", "98765432100", "maria@example.com", usuario);

        cliente.Nome.Should().Be("Maria Santos");
        cliente.Cpf.Should().Be("98765432100");
        cliente.Email.Should().Be("maria@example.com");
        cliente.IdUsuarioCriacao.Should().Be(usuario);
    }

    [Fact]
    public void CriarCliente_DeveGerarId()
    {
        var cliente = CriarCliente();

        cliente.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public void CriarCliente_DeveEstarAtivo()
    {
        var cliente = CriarCliente();

        cliente.Ativo.Should().BeTrue();
    }

    [Fact]
    public void Inativar_DeveMudarAtivoParaFalso()
    {
        var cliente = CriarCliente();

        cliente.Inativar();

        cliente.Ativo.Should().BeFalse();
    }

    [Fact]
    public void CriarCliente_DeveRegistrarDataCriacao()
    {
        var antes = DateTime.UtcNow;

        var cliente = CriarCliente();

        cliente.DataCriacao.Should().BeOnOrAfter(antes);
    }
}
