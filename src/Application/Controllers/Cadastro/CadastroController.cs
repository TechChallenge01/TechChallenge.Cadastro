using Application.Gateways.Categorias;
using Application.Gateways.Clientes;
using Application.Gateways.Produtos;
using Application.Interfaces;
using Application.UseCases.Categorias;
using Application.UseCases.Clientes;
using Application.UseCases.Produtos;
using Shared.DTOs.Categorias.Request;
using Shared.DTOs.Clientes.Request;
using Shared.DTOs.Produtos.Request;
using Shared.Result;

namespace Application.Controllers.Cadastro;

public class CadastroController
{
    private readonly IProdutoDataSource _produtoDataSource;
    private readonly ICategoriaDataSource _categoriaDataSource;
    private readonly IClienteDataSource _clienteDataSource;

    public CadastroController(IProdutoDataSource produtoDataSource, ICategoriaDataSource categoriaDataSource, IClienteDataSource clienteDataSource)
    {
        _produtoDataSource = produtoDataSource;
        _categoriaDataSource = categoriaDataSource;
        _clienteDataSource = clienteDataSource;
    }

    public Task<ICommandResult> CriarProduto(CriarProdutoRequestDTO request, Guid idUsuario, IEventPublisher eventPublisher, CancellationToken ct)
        => CriarProdutoUseCase.Create(ProdutoGateway.Create(_produtoDataSource), eventPublisher).Run(request, idUsuario, ct);

    public Task<ICommandResult> AtualizarProduto(Guid id, AtualizarProdutoRequestDTO request, Guid idUsuario, IEventPublisher eventPublisher, CancellationToken ct)
        => AtualizarProdutoUseCase.Create(ProdutoGateway.Create(_produtoDataSource), eventPublisher).Run(id, request, idUsuario, ct);

    public Task<ICommandResult> InativarProduto(Guid id, Guid idUsuario, IEventPublisher eventPublisher, CancellationToken ct)
        => InativarProdutoUseCase.Create(ProdutoGateway.Create(_produtoDataSource), eventPublisher).Run(id, idUsuario, ct);

    public Task<ICommandResult> GetProdutoById(Guid id, CancellationToken ct)
        => GetProdutoByIdUseCase.Create(ProdutoGateway.Create(_produtoDataSource)).Run(id, ct);

    public Task<ICommandResult> GetProdutosPaginados(int page, int pageSize, CancellationToken ct)
        => GetProdutosPaginadosUseCase.Create(ProdutoGateway.Create(_produtoDataSource)).Run(page, pageSize, ct);

    public Task<ICommandResult> CriarCategoria(CriarCategoriaRequestDTO request, Guid idUsuario, CancellationToken ct)
        => CriarCategoriaUseCase.Create(CategoriaGateway.Create(_categoriaDataSource)).Run(request, idUsuario, ct);

    public Task<ICommandResult> GetAllCategorias(CancellationToken ct)
        => GetAllCategoriasUseCase.Create(CategoriaGateway.Create(_categoriaDataSource)).Run(ct);

    public Task<ICommandResult> CadastrarCliente(CadastrarClienteRequestDTO request, Guid idUsuario, IEventPublisher eventPublisher, CancellationToken ct)
        => CadastrarClienteUseCase.Create(ClienteGateway.Create(_clienteDataSource), eventPublisher).Run(request, idUsuario, ct);

    public Task<ICommandResult> GetClienteById(Guid id, CancellationToken ct)
        => GetClienteByIdUseCase.Create(ClienteGateway.Create(_clienteDataSource)).Run(id, ct);
}
