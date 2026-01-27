# Cinema API

Sistema de gerenciamento de cinema e compra de tickets com arquitetura **Monolito Modular**, utilizando princípios de **Clean Archi** e **DDD**.


## Arquitetura

O projeto segue uma arquitetura **Monolito Modular** com separação clara de responsabilidades:

```
src/
├── Cinema/              # Aplicação principal (API Gateway)
└── Modules/
    ├── Auth/            # Módulo de Autenticação
    ├── Movie/           # Módulo de Filmes
    ├── Tickets/         # Módulo de Ingressos
    ├── PaymentGateway/  # Módulo de Pagamentos
    └── Shared/          # Componentes compartilhados
```

Cada módulo segue a estrutura **Clean Architecture**:
- `Domain/` - Entidades, interfaces e regras de negócio
- `Application/` - Casos de uso, DTOs e serviços
- `Infra/` - Implementações de repositórios, banco de dados e migrações
- `WebApi/` - Controllers e configuração do módulo

## Tecnologias

- .NET 9
- Entity Framework Core
- SQL Server

## Como Executar


1. **Clone o repositório**
```bash
git clone https://github.com/thiagobqq/Cinema.git
cd Cinema
```

2. **Configure a connection string** em `src/Cinema/appsettings.json`

3. **Execute as migrações**
```bash
dotnet ef database update \
  --project src/Modules/Auth/Auth.csproj \
  --startup-project src/Cinema/Cinema.csproj \
  --context AuthDbContext

dotnet ef database update \
  --project src/Modules/Movie/Movie.csproj \
  --startup-project src/Cinema/Cinema.csproj \
  --context MovieDbContext
```

4. **Execute a aplicação**
```bash
dotnet run --project src/Cinema/Cinema.csproj
```

## Misc

### Registrar um novo módulo na solution
```bash
dotnet sln add src/Modules/NomeDoModulo/
```

### Listar módulos registrados
```bash
dotnet sln list
```

### Criar uma nova migração
```bash
dotnet ef migrations add NomeDaMigracao \
  --project src/Modules/NomeDoModulo/NomeDoModulo.csproj \
  --startup-project src/Cinema/Cinema.csproj \
  --output-dir Migrations \
  --context NomeDoContexto
```

### Aplicar migrações
```bash
dotnet ef database update \
  --project src/Modules/NomeDoModulo/NomeDoModulo.csproj \
  --startup-project src/Cinema/Cinema.csproj \
  --context NomeDoContexto
```
