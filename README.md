# TechChallenge.Cadastro — Microsserviço de Cadastro

Microsserviço responsável pelo CRUD de Produtos, Categorias e Clientes, parte do sistema TechChallenger (oficina mecânica).

## Responsabilidades

- Gerenciamento de Produtos (nome, descrição, preço, categoria)
- Gerenciamento de Categorias de produtos
- Gerenciamento de Clientes (CPF, nome, contato)
- Serviço de suporte — não participa diretamente da Saga de OS

## Arquitetura

Clean Architecture com 5 camadas: **Domain**, **Shared**, **Application**, **Infra**, **API**.

## Banco de Dados

- **MongoDB** — banco de dados NoSQL
- Coleções: `produtos`, `categorias`, `clientes`

## Padrão Saga — Coreografado via RabbitMQ

Este serviço **não participa** da Saga de OS. É um serviço de suporte que fornece dados de referência (Produtos, Clientes) para os demais serviços via API REST síncrona (chamados pelo BFF).

A arquitetura coreografada foi escolhida para os serviços de negócio (OS, Billing, Estoque, Execução) porque:
- Elimina o ponto único de falha de um orquestrador central
- Permite que cada serviço evolua independentemente
- O Cadastro, por ser um serviço de suporte sem participação no fluxo transacional, não precisa publicar/consumir eventos

## Executando localmente

```bash
# Pré-requisitos: Docker, .NET 10 SDK
docker compose up -d
dotnet run --project src/API
```

## Variáveis de ambiente

| Variável | Descrição |
|----------|-----------|
| `ConnectionStrings__MongoDB` | URI do MongoDB |
| `MongoDB__DatabaseName` | Nome do banco (default: `techchallenge_cadastro`) |
| `Jwt__Key` | Chave JWT |
| `Jwt__Issuer` | Issuer JWT (default: `TechChallenger`) |
| `Jwt__Audience` | Audience JWT (default: `TechChallenger`) |

## Endpoints principais

| Método | Rota | Descrição |
|--------|------|-----------|
| `GET` | `/produtos` | Listar produtos (paginado) |
| `POST` | `/produtos` | Criar produto |
| `GET` | `/produtos/{id}` | Buscar produto |
| `PUT` | `/produtos/{id}` | Atualizar produto |
| `DELETE` | `/produtos/{id}` | Inativar produto |
| `GET` | `/categorias` | Listar categorias |
| `POST` | `/categorias` | Criar categoria |
| `GET` | `/clientes` | Listar clientes |
| `POST` | `/clientes` | Criar cliente |
| `GET` | `/clientes/{id}` | Buscar cliente |
| `PUT` | `/clientes/{id}` | Atualizar cliente |

## Kubernetes

Manifestos em `k8s/`: `deployment.yaml`, `service.yaml`, `hpa.yaml`.

## CI/CD

| Workflow | Trigger | Descrição |
|----------|---------|-----------|
| `deploy.yml` | push main | Build, ECR, ECS deploy |
| `infra.yml` | push main (infra/) | Terraform apply |
| `quality.yml` | push/PR main | Testes + SonarCloud |
| `k8s-deploy.yml` | manual | Deploy para EKS |

## Testes

```bash
dotnet test tests/UnitTests
```

Cobertura mínima: **80%** — validada pelo SonarCloud.

**Configuração SonarCloud**: Secret `SONAR_TOKEN`, Variables `SONAR_PROJECT_KEY` e `SONAR_ORGANIZATION`.
