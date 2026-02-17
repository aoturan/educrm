# Output Rules

- Output only the files that are changed.
- For each file, output the full updated file content, not diffs.
- Do not include explanations, comments, or summaries outside the code.
- Preserve existing formatting, naming conventions, and project structure.
- Do not invent files or folders that do not already exist unless explicitly required.

---

# Copilot Backend Instructions (EduCRM / .NET 8 / PostgreSQL)

You are working on **EduCRM backend** (modular monolith) built with **.NET 8** and **PostgreSQL**.

## 1) Scope and Working Mode
- I will specify a **current Domain** (example: `Account`).
  Until I explicitly change it, all code changes must stay within that Domain’s module and the related WebApi contract/controller paths.
- I may also specify a **target layer constraint** (example: “only UseCases” or “only Repository”).
  If a layer constraint is provided, only touch that layer and the minimal required wiring. Otherwise, work end-to-end.
- Default behavior: when I ask for an endpoint change, implement it along the full path from  
  **WebApi Contract DTO → Controller → Application UseCase → Repository interface → Infrastructure implementation → Domain entities (only if needed)**.

## 2) Project Structure (Canonical Paths)

### Web API
- `Shared/WebApi/Contracts/[Domain]/`  
  WebApi request/response DTOs for the domain (e.g., `RegisterRequest.cs`, `RegisterResponse.cs`).
- `Shared/WebApi/Controllers/[Domain]Controller.cs`  
  Domain controllers (e.g., `AccountController.cs`).
- `Shared/WebApi/Validation/Validators/[Domain]/`  
  FluentValidation validators (e.g., `RegisterRequestValidator.cs`).
- `Shared/WebApi/Validation/`  
  Core validation infrastructure (`IRequestValidator.cs`, `RequestValidator.cs`).

### Domain Module (example: Account)
- `EduCrm.Modules.[Domain].Application/UseCases/...`  
  UseCases, grouped by folders.  
  Contains usecase DTOs, service interfaces, and implementations.
- `EduCrm.Modules.[Domain].Application/Repository`  
  Repository interfaces.
- `EduCrm.Modules.[Domain].Domain/`  
  Domain entities.
- `EduCrm.Modules.[Domain].Infrastructure/`  
  Implementations of Application interfaces (except usecases), including PostgreSQL repositories.

### Cross-domain Contracts
- `EduCrm.Modules.[Domain].Contracts`  
  Used **only for cross-domain queries**.  
  Cross-domain writes must go through the owning domain's usecases and repositories unless explicitly stated otherwise.

### Shared Infrastructure
- `Shared/Infrastructure/Configuration/`  
  EF Core entity configurations (`IEntityTypeConfiguration<T>` implementations).
- `Shared/Infrastructure/Persistence/`  
  DbContext and shared persistence infrastructure.
- `Shared/Infrastructure/`  
  Cross-cutting infrastructure concerns shared across all domains.

### Shared Kernel
- `Shared/SharedKernel/EduCrm.SharedKernel/Errors/`  
  Centralized error definitions and mappings:
  - `ErrorCodes.cs` - All error code constants
  - `ErrorHttpStatusMapper.cs` - Error to HTTP status code mapping
  - `CommonErrors.cs` - Common error factory methods
  - `[Domain]Errors.cs` - Domain-specific error factory methods
- `Shared/SharedKernel/EduCrm.SharedKernel/Results/`  
  Result types for operation outcomes.
- `Shared/SharedKernel/EduCrm.SharedKernel/Abstractions/`  
  Shared abstractions (e.g., `IClock`).


## 3) Rules for Changes
- Keep changes minimal and localized.
- Do not introduce MediatR, generic repositories, or heavy abstractions.
- Do not create automatic cross-domain side effects.
- Operational concepts (e.g., enrollment, follow-ups) are never auto-created unless explicitly requested.

## 4) Result, Exceptions, and Context
- A standard **Result** type exists.  
  Use it for business outcomes. Do not throw for expected business errors.
- **Global exception handling** exists.  
  Unexpected exceptions may bubble up.
- **Organization context / general context** exists.  
  All reads and writes must respect organization scope.

## 4.1) Error Handling Best Practices
- **Error codes and messages** are centralized in `Shared/SharedKernel/EduCrm.SharedKernel/Errors/`:
  - `ErrorCodes.cs` - All error code constants (pattern: `{domain}.{specific_error}`)
  - `ErrorHttpStatusMapper.cs` - Maps error codes to HTTP status codes
  - `CommonErrors.cs` - Factory methods for common cross-cutting errors
  - `[Domain]Errors.cs` - Domain-specific error factory methods (e.g., `AccountErrors.cs`)
- **Creating new errors**:
  1. Add error code constant to `ErrorCodes.cs`
  2. Add HTTP status mapping in `ErrorHttpStatusMapper.cs`
  3. Create factory method in appropriate `[Domain]Errors.cs` or `CommonErrors.cs`
  4. Use the factory method in use cases: `return Result.Fail(AccountErrors.EmailTaken(email));`
- **Never hardcode error codes or messages** in use cases, controllers, or other layers.
- **ProblemDetailsFactory** automatically uses `ErrorHttpStatusMapper` to convert errors to proper HTTP responses.

## 4.2) Transaction Management Best Practices
- **Use `IAppDbTransaction`** for operations requiring explicit transaction control.
- **Transaction interfaces** are defined in `Shared/Infrastructure/Persistence/`:
  - `IAppDbTransaction` - Factory for creating transaction scopes
  - `IAppDbTransactionScope` - Transaction scope with `CommitAsync`, `RollbackAsync`, and `DisposeAsync`
  - `IUnitOfWork` - For `SaveChangesAsync` operations
- **Always use try-catch with explicit rollback** for transaction operations:
  ```csharp
  await using var trx = await tx.BeginAsync(ct);
  
  try
  {
      // ... operations ...
      await uow.SaveChangesAsync(ct);
      await trx.CommitAsync(ct);
      
      return Result<T>.Success(...);
  }
  catch
  {
      await trx.RollbackAsync(ct);
      throw;
  }
  ```
- **When to use explicit transactions**:
  - Multiple related writes that must be atomic
  - Read-then-write patterns requiring consistent reads (use serializable isolation)
  - Operations across multiple repositories
- **When NOT needed**:
  - Single `SaveChangesAsync` call (EF Core wraps it automatically)
  - Read-only operations
- **Business validation returning `Result.Fail`** does not require explicit rollback (no writes occurred).
- **Unexpected exceptions** in catch block should rollback and re-throw.

## 5) Endpoint Implementation Checklist (Default)
Unless a layer is explicitly restricted:
1. Update or create DTOs in `Shared/WebApi/Contracts/[Domain]/`.
2. Update or create FluentValidation validators in `Shared/WebApi/Validation/Validators/[Domain]/`.
3. Update the domain controller in `Shared/WebApi/Controllers/[Domain]Controller.cs`.
4. Implement or update the UseCase in `EduCrm.Modules.[Domain].Application/UseCases`.
5. Update repository interfaces in `EduCrm.Modules.[Domain].Application/Repository`.
6. Update PostgreSQL implementations in `EduCrm.Modules.[Domain].Infrastructure`.
7. Touch domain entities only if required.
8. Update DI wiring in `Shared/WebApi/Extensions/ServiceCollectionExtension.cs` only if new services or repositories are introduced.


## 6) Communication Protocol
- I will specify:
    - `Domain: <Name>`
    - Optional: `Layer: <UseCases | Repository | ...>`
    - Then the change request.
- If no domain is specified, assume the previously active domain.
- In ambiguous cases, choose the most conservative option:
    - avoid cross-domain writes,
    - stay within the active domain,
    - follow existing Result and context patterns.
