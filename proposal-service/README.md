Project/
├── src/
│ ├── ProposalService.Api/
│ │ ├── Controllers/
│ │ │ ├── HealthController.cs
│ │ │ └── ProposalsController.cs
│ │ ├── Contracts/
│ │ │ └── Proposals/ (Create/Update/Status DTOs)
│ │ ├── appsettings.json
│ │ └── Program.cs
│ │
│ ├── ProposalService.Application/
│ │ ├── Services/
│ │ │ └── ProposalService.cs (use cases)
│ │ └── Abstractions/ (ports inbound/outbound se quiser separar)
│ │
│ ├── ProposalService.Domain/
│ │ ├── Entities/
│ │ │ └── Proposal.cs
│ │ ├── Exceptions/
│ │ │ ├── NotFoundException.cs
│ │ │ └── BusinessException.cs
│ │ └── Interfaces/
│ │ └── IProposalRepository.cs
│ │
│ └── ProposalService.Infrastructure/
│ ├── Extensions/
│ │ ├── DependencyInjectionExtensions.cs
│ │ ├── HealthChecksExtensions.cs
│ │ └── DatabaseExtensions.cs
│ ├── Persistence/
│ │ ├── InMemory/ (para começar)
│ │ └── Dapper/ (depois)
│ └── Middleware/
│ └── GlobalExceptionHandler.cs
│
└── tests/
└── ProposalService.UnitTests/