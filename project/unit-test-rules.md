# Unit Test Rules — Nutricenter.ContractsBilling

Purpose: keep every unit test fast, deterministic, and reviewable so handlers and domain guards stay covered as the codebase grows.

Scope: a unit test covers one behavior with no I/O (no database, network, filesystem, or clock/timezone dependence). Anything touching real infrastructure is an integration test and lives outside these rules.

## Harness snapshot

| Tool | Pinned version |
|------|----------------|
| Target framework | `net10.0` (`Nullable` + `ImplicitUsings` enabled) |
| xUnit | `2.9.3` (+ `xunit.runner.visualstudio` `3.1.4`) |
| Moq | `4.20.72` |
| Coverage | `coverlet.collector` `6.0.4` |
| SDK | `Microsoft.NET.Test.Sdk` `17.14.1` |

```bash
dotnet test
dotnet test --collect:"XPlat Code Coverage"
```

## Folder and naming conventions

Mirror `src` layout inside `src/Nutricenter.ContractsBilling.Test`:

| Production code | Test location |
|-----------------|---------------|
| Domain aggregate/entity | `Domain/<Entity>Test.cs` |
| Application handler | `Application/Commands/<Aggregate>/<HandlerName>Test.cs` |

- Test files end with `Test.cs`; test classes match (`ContractTest`, `ContractCreateCommandHandlerTest`).
- Test methods use `MethodName_State_Expected` (e.g. `Handle_WithServices_ShouldCreateContractAndCommit`, `AddService_DuplicateReference_ShouldThrowDomainException`).
- `Xunit` is globally imported in the test `.csproj`; do not add `using Xunit;`.
- Share setup through `private static` factory helpers (`CreateCommand()`, `CreateContract()`, `CreateService()`), never shared mutable fields.

## Mandatory structure

- Every test follows Arrange / Act / Assert with one blank-separated block each and `// Arrange`, `// Act`, `// Assert` comments.
- One behavior per test: one Act, one logical assertion group.
- `[Fact]` is the default. Use `[Theory]` + `[InlineData]` only when the same behavior repeats over scalar inputs (validation ranges, guard variants).
- Async tests must be `public async Task <Name>Async`-style awaited code: `await handler.Handle(cmd, CancellationToken.None)`. Never use `.Result`, `.Wait()`, or `async void`.

## Mocking with Moq

- Mock ports only: repositories (`IContractRepository`), unit of work (`IUnitOfWork`), and domain services/generators (`IContractNumberGenerator`).
- Never mock domain entities, aggregates, or value objects — construct the real thing (`new Contract(...)`, `new SequenceNumberGenerator(...)`).
- Prefer explicit `Setup` + `ReturnsAsync` / `Callback` for capture, then `Verify(..., Times.Once)` on exactly one commit path per success test: `CreateAsync` on the repository and `CommitAsync` on the unit of work.
- Verify with precise matchers where identity matters (`It.Is<T>(...)` on code/name); use `It.IsAny<T>()` only for pass-through arguments. Do not over-mock value objects or assert incidental calls.

## Data and edge-case rules

- Every guarded handler needs success + not-found + invalid-state cases where the guard exists; every domain guard needs both branches (allowed transition and `DomainException`).
- Cover empty / null / invalid-state inputs for every guard clause; use fixed deterministic values (`new DateOnly(2026, 1, 1)`, `"CTR-2026-00001"`).
- No magic strings scattered in bodies: hoist repeated ids/descriptions into factory defaults or `const` locals.
- No shared mutable state between tests; each test builds its own arrange graph.
- No I/O, network, real clock, or timezone dependence — inject or fix all inputs.

## Assertions and coverage

- Assert outcomes, not internals: `result.IsSuccess`, state transitions (`ContractStatus.Active`), collection shape (`Assert.Single`), captured aggregate contents.
- Guard violations assert `Assert.Throws<DomainException>(...)`.
- Expected minimum per command handler: success path with `CreateAsync` + `CommitAsync` verified `Times.Once`; failure paths assert no commit.
- Expected minimum per domain guard: happy-path state assertion plus throwing case for each illegal state.

## Anti-patterns

- Multiple behaviors or Acts in one test.
- `.Result` / `.Wait()` / `async void` in async tests.
- Mocking domain entities or value objects.
- Verifying incidental calls or overspecifying every collaborator interaction.
- Shared mutable fixtures or static state across tests.
- Magic strings/ids duplicated instead of factory helpers.
- Real I/O, network, clock, or timezone dependence.
- Assertion-free or `Assert.True(true)` placeholder tests.

## Skeleton example

```csharp
[Fact]
public async Task Handle_WithServices_ShouldCreateContractAndCommit()
{
    // Arrange
    var repositoryMock = new Mock<IContractRepository>();
    var numberGeneratorMock = new Mock<IContractNumberGenerator>();
    var unitOfWorkMock = new Mock<IUnitOfWork>();
    var handler = new ContractCreateCommandHandler(
        repositoryMock.Object, numberGeneratorMock.Object, unitOfWorkMock.Object);
    var command = CreateCommand();

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.True(result.IsSuccess);
    repositoryMock.Verify(x => x.CreateAsync(It.IsAny<ContractAggregate>()), Times.Once);
    unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
}
```

Status: draft — pending team review
Runner: dotnet test
