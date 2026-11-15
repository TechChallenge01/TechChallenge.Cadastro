using Application.Gateways.Clientes;
using Application.Interfaces;
using Application.UseCases.Clientes;
using FluentAssertions;
using Moq;
using Shared.DTOs.Clientes.Input;
using System.Net;

namespace UnitTests.Application.UseCases;

public class GetClienteByIdUseCaseTests
{
    private readonly Mock<IClienteDataSource> _mockDS = new();

    private GetClienteByIdUseCase CriarUseCase()
        => GetClienteByIdUseCase.Create(ClienteGateway.Create(_mockDS.Object));

    [Fact]
    public async Task Run_QuandoClienteExiste_DeveRetornarOk()
    {
        var dto = new ClienteInputDTO { Id = Guid.NewGuid(), Nome = "João", Cpf = "123", Email = "j@t.com", IdUsuarioCriacao = Guid.NewGuid(), DataCriacao = DateTime.UtcNow, Ativo = true };
        _mockDS.Setup(x => x.GetById(dto.Id, It.IsAny<CancellationToken>())).ReturnsAsync(dto);

        var result = await CriarUseCase().Run(dto.Id, CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Run_QuandoClienteNaoExiste_DeveRetornarNotFound()
    {
        _mockDS.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ClienteInputDTO?)null);

        var result = await CriarUseCase().Run(Guid.NewGuid(), CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
