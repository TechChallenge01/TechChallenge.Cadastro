using Application.Gateways.Produtos;
using Application.Interfaces;
using Application.UseCases.Produtos;
using FluentAssertions;
using Moq;
using Shared.DTOs.Produtos.Input;
using Shared.DTOs.Produtos.Request;
using System.Net;

namespace UnitTests.Application.UseCases;

public class CriarProdutoUseCaseTests
{
    private readonly Mock<IProdutoDataSource> _mockDS = new();
    private readonly Mock<IEventPublisher> _mockPublisher = new();

    private CriarProdutoUseCase CriarUseCase()
        => CriarProdutoUseCase.Create(ProdutoGateway.Create(_mockDS.Object), _mockPublisher.Object);

    [Fact]
    public async Task Run_DeveCriarProdutoEPublicarEvento()
    {
        _mockDS.Setup(x => x.Create(It.IsAny<ProdutoInputDTO>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockPublisher.Setup(x => x.PublishAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var request = new CriarProdutoRequestDTO { Nome = "Produto X", Descricao = "Desc", Preco = 99.90m, CategoriaId = Guid.NewGuid() };
        var result = await CriarUseCase().Run(request, Guid.NewGuid(), CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.Created);
        _mockPublisher.Verify(x => x.PublishAsync("cadastro.produto.criado", It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
