using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Gateways.Clientes;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.EndPoints.Auth;

public class AutenticarPorCpf : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/cpf", async (
            [FromBody] AutenticarRequestDTO request,
            IClienteDataSource clienteDataSource,
            IConfiguration config,
            CancellationToken ct) =>
        {
            if (string.IsNullOrWhiteSpace(request.Cpf))
                return Results.BadRequest(new { message = "CPF é obrigatório." });

            var cpf = request.Cpf.Replace(".", "").Replace("-", "").Replace("/", "");

            var gateway = ClienteGateway.Create(clienteDataSource);
            var cliente = await gateway.GetByCpf(cpf, ct);

            if (cliente is null)
                return Results.NotFound(new { message = "Cliente não encontrado para o CPF informado." });

            if (!cliente.Ativo)
                return Results.Problem(
                    statusCode: 403,
                    detail: "Cliente inativo.");

            var key    = config["Jwt:Key"]!;
            var issuer = config["Jwt:Issuer"] ?? "TechChallenger";
            var aud    = config["Jwt:Audience"] ?? "TechChallenger";
            var horas  = int.TryParse(config["Jwt:ExpiracaoHoras"], out var h) ? h : 8;

            var expiracao = DateTime.UtcNow.AddHours(horas);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,   cliente.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString()),
                new Claim("cpf",   cliente.Cpf),
                new Claim("nome",  cliente.Nome),
                new Claim(ClaimTypes.Email, cliente.Email),
            };

            var creds  = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer:   issuer,
                audience: aud,
                claims:   claims,
                expires:  expiracao,
                signingCredentials: creds);

            return Results.Ok(new
            {
                token      = new JwtSecurityTokenHandler().WriteToken(token),
                expiracao,
                clienteId  = cliente.Id,
                nome       = cliente.Nome,
            });
        })
        .AllowAnonymous()
        .WithTags("Auth")
        .WithSummary("Autentica cliente via CPF e retorna JWT");
    }
}

public record AutenticarRequestDTO(string Cpf);
