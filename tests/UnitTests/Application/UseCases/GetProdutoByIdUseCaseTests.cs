using Application.Gateways.Produtos;
using Application.Interfaces;
using Application.UseCases.Produtos;
using FluentAssertions;
using Moq;
using Shared.DTOs.Produtos.Input;
using System.Net;

namespace UnitTests.Application.UseCases;

public class GetProdutoByIdUseCaseTests
{
    private readonly Mock<IProdutoDataSource> _mockDS = new();

    private GetProdutoByIdUseCase CriarUseCase()
        => GetProdutoByIdUseCase.Create(ProdutoGateway.Create(_mockDS.Object));

    [Fact]
    public async Task Run_QuandoProdutoExiste_DeveRetornarOk()
    {
        var dto = new ProdutoInputDTO { Id = Guid.NewGuid(), Nome = "P", Descricao = "D", Preco = 10m, CategoriaId = Guid.NewGuid(), IdUsuarioCriacao = Guid.NewGuid(), DataCriacao = DateTime.UtcNow, Ativo = true };
        _mockDS.Setup(x => x.GetById(dto.Id, It.IsAny<CancellationToken>())).ReturnsAsync(dto);

        var result = await CriarUseCase().Run(dto.Id, CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Run_QuandoProdutoNaoExiste_DeveRetornarNotFound()
    {
        _mockDS.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProdutoInputDTO?)null);

        var result = await CriarUseCase().Run(Guid.NewGuid(), CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
