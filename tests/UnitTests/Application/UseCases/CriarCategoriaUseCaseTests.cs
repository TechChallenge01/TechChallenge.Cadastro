using Application.Gateways.Categorias;
using Application.Interfaces;
using Application.UseCases.Categorias;
using FluentAssertions;
using Moq;
using Shared.DTOs.Categorias.Input;
using Shared.DTOs.Categorias.Request;
using System.Net;

namespace UnitTests.Application.UseCases;

public class CriarCategoriaUseCaseTests
{
    private readonly Mock<ICategoriaDataSource> _mockDS = new();

    private CriarCategoriaUseCase CriarUseCase()
        => CriarCategoriaUseCase.Create(CategoriaGateway.Create(_mockDS.Object));

    [Fact]
    public async Task Run_DeveCriarCategoriaERetornarCreated()
    {
        _mockDS.Setup(x => x.Create(It.IsAny<CategoriaInputDTO>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var request = new CriarCategoriaRequestDTO { Nome = "Eletrônicos" };
        var result = await CriarUseCase().Run(request, Guid.NewGuid(), CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.Created);
        _mockDS.Verify(x => x.Create(It.IsAny<CategoriaInputDTO>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
