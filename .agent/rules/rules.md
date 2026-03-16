# NYC360.Backend - Project Rules

## Architecture & Patterns
- **Clean Architecture**: 4 Layers (API, Application, Domain, Infrastructure).
- **Web API**: FastEndpoints + MediatR.
- **CQRS**: Feature folders. Commands, Handlers, and Validators MUST be in separate files.
- **Data Access**: Repository Pattern (Generic + Specific) + Unit of Work (`SaveChangesAsync`).
- **Dependency Injection**: Use Primary Constructors (C# 12).

## Coding Conventions
- **Asynchronous**: Use `async`/`await` with `CancellationToken` (ct).
- **Naming**: PascalCase for classes/methods, camelCase for variables/parameters.
- **Responses**: Always use `StandardResponse` or `StandardResponse<T>`.
- **Error Handling**: Use `StandardResponse.Failure(new ApiError("entity.reason", "Message"))`.

## Layers
- **Domain**: Pure entities, enums, DTOs, wrappers. No dependencies.
- **Application**: Features, MediatR logic, Persistence contracts.
- **Infrastructure**: DB implementation, External services, Migrations.
- **API**: Endpoints, Web config, Swagger.
