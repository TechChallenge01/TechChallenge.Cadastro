using Application.Gateways.Produtos;
using Application.Interfaces;
using Application.UseCases.Produtos;
using FluentAssertions;
using Moq;
using Shared.DTOs.Produtos.Input;
using System.Net;

namespace UnitTests.Application.UseCases;

public class InativarProdutoUseCaseTests
{
    private readonly Mock<IProdutoDataSource> _mockDS = new();
    private readonly Mock<IEventPublisher> _mockPublisher = new();

    private InativarProdutoUseCase CriarUseCase()
        => InativarProdutoUseCase.Create(ProdutoGateway.Create(_mockDS.Object), _mockPublisher.Object);

    [Fact]
    public async Task Run_QuandoProdutoExiste_DeveInativarEPublicarEvento()
    {
        var dto = new ProdutoInputDTO { Id = Guid.NewGuid(), Nome = "Prod", Descricao = "D", Preco = 10m, CategoriaId = Guid.NewGuid(), IdUsuarioCriacao = Guid.NewGuid(), DataCriacao = DateTime.UtcNow, Ativo = true };
        _mockDS.Setup(x => x.GetById(dto.Id, It.IsAny<CancellationToken>())).ReturnsAsync(dto);
        _mockDS.Setup(x => x.Update(It.IsAny<ProdutoInputDTO>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockPublisher.Setup(x => x.PublishAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await CriarUseCase().Run(dto.Id, Guid.NewGuid(), CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        _mockPublisher.Verify(x => x.PublishAsync("cadastro.produto.inativado", It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Run_QuandoProdutoNaoExiste_DeveRetornarNotFound()
    {
        _mockDS.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProdutoInputDTO?)null);

        var result = await CriarUseCase().Run(Guid.NewGuid(), Guid.NewGuid(), CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
