using Application.Gateways.Produtos;
using Application.Interfaces;
using Application.UseCases.Produtos;
using FluentAssertions;
using Moq;
using Shared.DTOs.Produtos.Input;
using System.Net;

namespace UnitTests.Application.UseCases;

public class GetProdutosPaginadosUseCaseTests
{
    private readonly Mock<IProdutoDataSource> _mockDS = new();

    private GetProdutosPaginadosUseCase CriarUseCase()
        => GetProdutosPaginadosUseCase.Create(ProdutoGateway.Create(_mockDS.Object));

    [Fact]
    public async Task Run_DeveRetornarProdutosPaginados()
    {
        var dtos = new List<ProdutoInputDTO>
        {
            new() { Id = Guid.NewGuid(), Nome = "P1", Descricao = "D1", Preco = 10m, CategoriaId = Guid.NewGuid(), IdUsuarioCriacao = Guid.NewGuid(), DataCriacao = DateTime.UtcNow, Ativo = true },
            new() { Id = Guid.NewGuid(), Nome = "P2", Descricao = "D2", Preco = 20m, CategoriaId = Guid.NewGuid(), IdUsuarioCriacao = Guid.NewGuid(), DataCriacao = DateTime.UtcNow, Ativo = true }
        };
        _mockDS.Setup(x => x.GetPaginated(1, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync((dtos, 2));

        var result = await CriarUseCase().Run(1, 10, CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.PartialContent);
    }
}
