# TechChallenge.Cadastro — Microsserviço de Cadastro

> Fase 4 — FIAP SOAT · Arquitetura de Microsserviços com Saga Pattern

Microsserviço responsável pelo **cadastro e autenticação de clientes**, além do gerenciamento de **produtos e categorias** utilizados pelos demais serviços. É o ponto de autenticação do sistema — emite os tokens JWT utilizados em todos os outros serviços.

---

## 🔗 Repositórios do TechChallenger

| Serviço | Repositório | Responsabilidade |
|---------|-------------|------------------|
| OS Service | [TechChallenge.OS](https://github.com/TechChallenge01/TechChallenge.OS) | Ciclo de vida das Ordens de Serviço |
| Billing Service | [TechChallenge.Billing](https://github.com/TechChallenge01/TechChallenge.Billing) | Orçamentos e pagamentos (Mercado Pago) |
| **Cadastro Service** ← *este* | [TechChallenge.Cadastro](https://github.com/TechChallenge01/TechChallenge.Cadastro) | Clientes, produtos e categorias |
| Estoque Service | [TechChallenge.Estoque](https://github.com/TechChallenge01/TechChallenge.Estoque) | Reserva e liberação de peças |
| Execução Service | [TechChallenge.Execucao](https://github.com/TechChallenge01/TechChallenge.Execucao) | Fila de execução e reparos |
| BFF | [TechChallenge.bff](https://github.com/TechChallenge01/TechChallenge.bff) | Gateway + Frontend React |

---

## 🏗️ Arquitetura deste repositório

```mermaid
flowchart TD
    Client([Cliente / BFF]) -->|HTTP| API[API Layer\nMinimal API .NET 10]
    API --> App[Application Layer\nControllers · UseCases]
    App --> Domain[Domain Layer\nCliente · Produto · Categoria]
    App --> Infra[Infra Layer\nMongoDB · JWT]
    Infra --> DB[(MongoDB\ncadastro)]
    API -->|emite token JWT| Client
```

**Camadas (Clean Architecture):**

| Camada | Conteúdo |
|--------|----------|
| `Domain` | `Cliente`, `Produto`, `Categoria`, regras de negócio |
| `Shared` | DTOs, interfaces compartilhadas |
| `Application` | Controllers, UseCases (cadastrar cliente, criar produto, autenticar) |
| `Infra` | MongoDB, geração de JWT |
| `API` | Endpoints Minimal API, Program.cs |

> **Nota:** O Cadastro não participa diretamente da Saga via RabbitMQ — atua como serviço de suporte (autenticação e dados cadastrais) consumido sincronicamente pelos demais.

**Deploy:** AWS ECS Fargate · cluster `techchallenge` · região `us-east-1`

---

## 🎯 Objetivo da Fase 4

- Refatorar a aplicação monolítica em microsserviços independentes
- Centralizar autenticação e emissão de JWT em serviço dedicado
- Gerenciar o catálogo de produtos e clientes de forma independente
- Pipeline CI/CD independente com testes automatizados e deploy no ECS Fargate
- Monitoramento e observabilidade com **CloudWatch** e **Datadog**

---

## 🔐 Autenticação

O Cadastro é o **provedor de identidade** do sistema. Todos os demais serviços validam o token JWT emitido aqui.

```http
POST /auth/cpf
Content-Type: application/json

{
  "cpf": "12345678900"
}
```

**Resposta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 3600
}
```

Use o token nos demais serviços:
```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

---

## ⚙️ Pré-requisitos

**Execução local:**
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- MongoDB 7.0 (via Docker ou Atlas)

**Infraestrutura AWS:**
- AWS CLI configurado (`aws configure`)
- Terraform ≥ 1.9
- Acesso ao cluster ECS `techchallenge` em `us-east-1`

---

## 🚀 Como Executar

### Local com Docker Compose

```bash
git clone https://github.com/TechChallenge01/TechChallenge.Cadastro.git
cd TechChallenge.Cadastro
docker compose up -d
```

Acesse: `http://localhost:5004/scalar` (Scalar API Docs)

### Testes

```bash
dotnet test tests/UnitTests
```

---

## 🔑 Variáveis de Ambiente

| Variável | Descrição | Exemplo |
|----------|-----------|---------|
| `MongoDB__ConnectionString` | URI do MongoDB | `mongodb://localhost:27017` |
| `MongoDB__DatabaseName` | Nome do banco | `cadastro` |
| `Jwt__Key` | Chave JWT (mín. 32 chars) — **deve ser igual em todos os serviços** | — |
| `Jwt__Issuer` | Issuer JWT | `TechChallenger` |
| `Jwt__Audience` | Audience JWT | `TechChallenger` |

> ⚠️ A `Jwt__Key` deve ser **idêntica** em todos os microsserviços para que os tokens gerados pelo Cadastro sejam aceitos pelos demais.

---

## 📡 Endpoints da API

| Método | Rota | Autenticação | Descrição |
|--------|------|--------------|-----------|
| `GET` | `/health` | — | Health check |
| `POST` | `/auth/cpf` | — | Autenticar por CPF, retorna JWT |
| `POST` | `/clientes` | Bearer | Cadastrar novo cliente |
| `GET` | `/clientes/{id}` | Bearer | Buscar cliente por ID |
| `GET` | `/categorias` | Bearer | Listar categorias |
| `POST` | `/categorias` | Bearer | Criar categoria |
| `GET` | `/produtos` | Bearer | Listar produtos (paginado) |
| `GET` | `/produtos/{id}` | Bearer | Buscar produto por ID |
| `POST` | `/produtos` | Bearer | Criar produto |
| `PUT` | `/produtos/{id}` | Bearer | Atualizar produto |
| `DELETE` | `/produtos/{id}` | Bearer | Inativar produto |

**Exemplo — Cadastrar cliente:**
```json
POST /clientes
{
  "nome": "João da Silva",
  "cpf": "12345678900",
  "email": "joao@email.com",
  "telefone": "11999999999"
}
```

**Exemplo — Criar produto:**
```json
POST /produtos
{
  "nome": "Vela de ignição NGK",
  "categoriaId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "preco": 25.90,
  "unidade": "un"
}
```

---

## 🏭 Infraestrutura

**Deploy:** AWS ECS Fargate · cluster `techchallenge` · `us-east-1`

Infraestrutura como código em `infra/` (Terraform):

| Arquivo | Recurso |
|---------|---------|
| `ecs.tf` | ECS Service + Task Definition |
| `ecr.tf` | Repositório ECR para a imagem Docker |
| `alb.tf` | Application Load Balancer |
| `cloudwatch.tf` | Log groups `/ecs/cadastro-service` |
| `data-sources.tf` | Data sources (VPC, subnets, cluster ECS) |

---

## 📦 Kubernetes

Manifestos em `k8s/`:

| Arquivo | Recurso |
|---------|---------|
| `deployment.yaml` | 2 réplicas, liveness/readiness probes |
| `service.yaml` | ClusterIP porta 80 |
| `hpa.yaml` | Auto-scaling 2–5 pods (70% CPU) |

---

## 🔁 CI/CD

| Workflow | Trigger | Jobs |
|----------|---------|------|
| `deploy.yml` | push `main` (src/) | Build → Push ECR → Deploy ECS |
| `quality.yml` | push/PR `main` | Testes unitários → SonarCloud |
| `infra.yml` | push `main` (infra/) | Terraform plan/apply |
| `k8s-deploy.yml` | manual (`workflow_dispatch`) | Deploy EKS |

---

## 🧪 Testes

```bash
dotnet test tests/UnitTests

# Com cobertura
dotnet test tests/UnitTests --collect:"XPlat Code Coverage" --results-directory coverage/
```

- **Cobertura:** fluxos críticos de cadastro (cliente, produto, autenticação) — relatório no SonarCloud
- **Relatório:** artefato `coverage-report-cadastro` em cada execução do `quality.yml`

---

## 📚 Documentação da API

Scalar UI disponível em:

- **Local:** `http://localhost:5004/scalar`
- **Produção:** via BFF ALB

---

## 📬 Collection Postman

Collection completa (todos os serviços) disponível em:

📂 [`TechChallenge.bff/docs/TechChallenger-Postman-Collection.json`](https://github.com/TechChallenge01/TechChallenge.bff/blob/main/docs/TechChallenger-Postman-Collection.json)

---

## 🗂️ Estrutura do Projeto

```
TechChallenge.Cadastro/
├── src/
│   ├── Domain/          # Cliente, Produto, Categoria, regras
│   ├── Shared/          # DTOs, interfaces compartilhadas
│   ├── Application/     # Controllers, UseCases
│   ├── Infra/           # MongoDB, JWT publisher
│   └── API/             # Endpoints, Program.cs
├── tests/
│   └── UnitTests/       # Testes unitários (xUnit)
├── k8s/                 # Manifestos Kubernetes
├── infra/               # Terraform (ECS, ECR, ALB, CloudWatch)
└── .github/workflows/   # CI/CD pipelines
```
