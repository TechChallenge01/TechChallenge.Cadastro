using Application.Gateways.Clientes;
using Application.Interfaces;
using Application.UseCases.Clientes;
using FluentAssertions;
using Moq;
using Shared.DTOs.Clientes.Input;
using Shared.DTOs.Clientes.Request;
using System.Net;

namespace UnitTests.Application.UseCases;

public class CadastrarClienteUseCaseTests
{
    private readonly Mock<IClienteDataSource> _mockDS = new();
    private readonly Mock<IEventPublisher> _mockPublisher = new();

    private CadastrarClienteUseCase CriarUseCase()
        => CadastrarClienteUseCase.Create(ClienteGateway.Create(_mockDS.Object), _mockPublisher.Object);

    [Fact]
    public async Task Run_QuandoEmailNaoExiste_DeveCriarClienteERetornarCreated()
    {
        _mockDS.Setup(x => x.GetByEmail(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ClienteInputDTO?)null);
        _mockDS.Setup(x => x.Create(It.IsAny<ClienteInputDTO>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockPublisher.Setup(x => x.PublishAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var request = new CadastrarClienteRequestDTO { Nome = "João", Cpf = "12345678901", Email = "joao@test.com" };
        var result = await CriarUseCase().Run(request, Guid.NewGuid(), CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.Created);
        _mockPublisher.Verify(x => x.PublishAsync("cadastro.cliente.cadastrado", It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Run_QuandoEmailJaExiste_DeveRetornarBadRequest()
    {
        _mockDS.Setup(x => x.GetByEmail("joao@test.com", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ClienteInputDTO { Id = Guid.NewGuid(), Nome = "João", Cpf = "123", Email = "joao@test.com", IdUsuarioCriacao = Guid.NewGuid(), DataCriacao = DateTime.UtcNow, Ativo = true });

        var request = new CadastrarClienteRequestDTO { Nome = "João2", Cpf = "99999999999", Email = "joao@test.com" };
        var result = await CriarUseCase().Run(request, Guid.NewGuid(), CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        _mockPublisher.Verify(x => x.PublishAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
