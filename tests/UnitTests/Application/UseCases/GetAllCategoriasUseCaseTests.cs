using Application.Gateways.Categorias;
using Application.Interfaces;
using Application.UseCases.Categorias;
using FluentAssertions;
using Moq;
using Shared.DTOs.Categorias.Input;
using System.Net;

namespace UnitTests.Application.UseCases;

public class GetAllCategoriasUseCaseTests
{
    private readonly Mock<ICategoriaDataSource> _mockDS = new();

    private GetAllCategoriasUseCase CriarUseCase()
        => GetAllCategoriasUseCase.Create(CategoriaGateway.Create(_mockDS.Object));

    [Fact]
    public async Task Run_DeveRetornarListaDeCategorias()
    {
        _mockDS.Setup(x => x.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CategoriaInputDTO>
            {
                new() { Id = Guid.NewGuid(), Nome = "Cat1", IdUsuarioCriacao = Guid.NewGuid(), DataCriacao = DateTime.UtcNow, Ativo = true },
                new() { Id = Guid.NewGuid(), Nome = "Cat2", IdUsuarioCriacao = Guid.NewGuid(), DataCriacao = DateTime.UtcNow, Ativo = true }
            });

        var result = await CriarUseCase().Run(CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Run_QuandoListaVazia_DeveRetornarOk()
    {
        _mockDS.Setup(x => x.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CategoriaInputDTO>());

        var result = await CriarUseCase().Run(CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
