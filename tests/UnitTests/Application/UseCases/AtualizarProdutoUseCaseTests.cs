using Application.Gateways.Produtos;
using Application.Interfaces;
using Application.UseCases.Produtos;
using FluentAssertions;
using Moq;
using Shared.DTOs.Produtos.Input;
using Shared.DTOs.Produtos.Request;
using System.Net;

namespace UnitTests.Application.UseCases;

public class AtualizarProdutoUseCaseTests
{
    private readonly Mock<IProdutoDataSource> _mockDS = new();
    private readonly Mock<IEventPublisher> _mockPublisher = new();

    private AtualizarProdutoUseCase CriarUseCase()
        => AtualizarProdutoUseCase.Create(ProdutoGateway.Create(_mockDS.Object), _mockPublisher.Object);

    private static ProdutoInputDTO ProdutoDTO() => new()
    {
        Id = Guid.NewGuid(), Nome = "Produto A", Descricao = "Desc A", Preco = 50m,
        CategoriaId = Guid.NewGuid(), IdUsuarioCriacao = Guid.NewGuid(),
        DataCriacao = DateTime.UtcNow, Ativo = true
    };

    [Fact]
    public async Task Run_QuandoProdutoExiste_DeveAtualizarEPublicarEvento()
    {
        var dto = ProdutoDTO();
        _mockDS.Setup(x => x.GetById(dto.Id, It.IsAny<CancellationToken>())).ReturnsAsync(dto);
        _mockDS.Setup(x => x.Update(It.IsAny<ProdutoInputDTO>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockPublisher.Setup(x => x.PublishAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var request = new AtualizarProdutoRequestDTO { Nome = "Novo Nome", Descricao = "Nova Desc", Preco = 75m, CategoriaId = Guid.NewGuid() };
        var result = await CriarUseCase().Run(dto.Id, request, Guid.NewGuid(), CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        _mockPublisher.Verify(x => x.PublishAsync("cadastro.produto.atualizado", It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Run_QuandoProdutoNaoExiste_DeveRetornarNotFound()
    {
        _mockDS.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProdutoInputDTO?)null);

        var request = new AtualizarProdutoRequestDTO { Nome = "X", Descricao = "Y", Preco = 10m, CategoriaId = Guid.NewGuid() };
        var result = await CriarUseCase().Run(Guid.NewGuid(), request, Guid.NewGuid(), CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
