# Unit Test Rules v1 — Nutricenter.ContractsBilling

Companion to `project/unit-test-rules.md`. This file is the **normative checklist**: every rule below is testable by review, and a PR cannot be considered ready if any mandatory rule is broken.

- Purpose: keep unit tests deterministic, fast, and reviewable so domain guards and command handlers stay protected as the codebase grows.
- Unit test: one behavior, no I/O (no database, network, filesystem, real clock or timezone). Anything touching real infrastructure is an integration test and is out of scope here.
- Evidence base: rules are derived from `Domain/ContractTest.cs` and `Application/Commands/Contract/*Test.cs`, not from generic convention.

## 1. Harness

| Tool | Pinned version |
|------|----------------|
| Target framework | `net10.0` (`Nullable` + `ImplicitUsings` enabled) |
| xUnit | `2.9.3` (+ `xunit.runner.visualstudio` `3.1.4`) |
| Mocking | `Moq` `4.20.72` |
| Coverage | `coverlet.collector` `6.0.4` |
| SDK | `Microsoft.NET.Test.Sdk` `17.14.1` |

```bash
dotnet test src/Nutricenter.ContractsBilling.Test
dotnet test src/Nutricenter.ContractsBilling.Test --collect:"XPlat Code Coverage"
```

**Rule 1.1 (mandatory):** the full suite must be green (`0 failed`, `0 skipped`) before any commit.
**Rule 1.2 (mandatory):** do not add a new test framework or assertion library without updating this file in the same PR.

## 2. Layout and naming

Mirror `src` inside `src/Nutricenter.ContractsBilling.Test`:

| Production code | Test location |
|-----------------|---------------|
| Domain aggregate/entity/value object | `Domain/<Type>Test.cs` |
| Application command handler | `Application/Commands/<Area>/<Handler>Test.cs` |

**Rule 2.1 (mandatory):** file name = type under test + `Test.cs`. Class name matches file name.
**Rule 2.2 (mandatory):** method name = `Method_WhereCondition_ExpectedOutcome`, e.g. `Activate_EmptyDraftContract_ShouldThrowDomainException`, `Handle_WithServices_ShouldCreateContractAndCommit`.
**Rule 2.3 (mandatory):** namespace mirrors folder: `Nutricenter.ContractsBilling.Test.Application.Commands.Contract`.
**Rule 2.4:** `Xunit` is globally imported via `<Using Include="Xunit" />` — never write `using Xunit;`.
**Rule 2.5:** only files that contain at least one `[Fact]`/`[Theory]` may live in the test project. Delete placeholder tests such as `UnitTest1.cs`.

## 3. Structure (AAA)

**Rule 3.1 (mandatory):** every test follows Arrange → Act → Assert in that order, one logical assertion group per test. Exactly **one Act**.
**Rule 3.2 (mandatory):** use `// Arrange`, `// Act`, `// Assert` comments when the arrangement exceeds 3 lines (all command handler tests). Short domain tests may omit them when the body reads top-to-bottom already.
**Rule 3.3 (mandatory):** `[Fact]` is the default. Use `[Theory]` + `[InlineData]` only when one behavior repeats over scalar inputs (validation ranges, guard variants). Do not use `[Theory]` to avoid writing distinct tests for distinct behaviors.
**Rule 3.4 (mandatory):** async tests `await` the call directly (`await handler.Handle(cmd, CancellationToken.None)`). Never `.Result`, `.Wait()`, or `async void`.
**Rule 3.5:** test names and bodies are in English; no persona or chat language leaks into the test code.

## 4. Test data

**Rule 4.1 (mandatory):** build inputs with `private static` factory helpers carrying defaults — `CreateContract(number = "CTR-2026-00001")`, `CreateCommand()`, `CreateService(...)`. Never assemble the same object inline in multiple tests.
**Rule 4.2 (mandatory):** no shared mutable fields. Each test builds its own arrange graph; static helpers must be pure.
**Rule 4.3 (mandatory):** deterministic values only — fixed dates (`new DateOnly(2026, 1, 1)`), fixed references (`"SVC-001"`), `Guid.NewGuid()` only for identity that is never asserted.
**Rule 4.4 (mandatory):** no `DateTime.Now`/`DateTime.UtcNow`/`Environment.*` in tests. If production code depends on the clock, inject it and fix the value in arrange.
**Rule 4.5:** no magic strings duplicated across the body: hoist into factory defaults or `const` locals.

## 5. Mocking with Moq

**Rule 5.1 (mandatory):** mock **ports only** — repositories (`IContractRepository`), unit of work (`IUnitOfWork`), domain services/generators (`IContractNumberGenerator`).
**Rule 5.2 (mandatory):** never mock domain entities, aggregates, or value objects. Construct the real thing (`new Contract(...)`, `new SequenceNumberGenerator(...)`). Domain logic must run for real.
**Rule 5.3 (mandatory):** every success-path handler test verifies persistence exactly once: `Verify(..., Times.Once)` on the repository call the handler actually makes (`CreateAsync`, `UpdateAsync`, `DeleteAsync`, ...), plus `Verify(x => x.CommitAsync(...), Times.Once)`.
**Rule 5.4 (mandatory):** every failure-path handler test asserts **no commit**: `unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never)`.
**Rule 5.5 (mandatory):** use precise matchers where identity carries meaning — `It.Is<T>(...)` on code/name/status. `It.IsAny<T>()` only for pass-through arguments.
**Rule 5.6:** capture out/aggregate state with `.Callback<T>(x => created = x)` and assert the captured object's contents instead of verifying incidental collaborator calls.
**Rule 5.7:** no over-mocking — asserting every method call couples the test to the implementation and breaks on refactors.

## 6. Coverage expectations

**Rule 6.1 (mandatory) — domain guards:** both branches for every guard: legal transition asserts the resulting status (`ContractStatus.Active`), illegal transition asserts `Assert.Throws<DomainException>(...)`.
**Rule 6.2 (mandatory) — command handlers:** minimum three cases where the guard exists: success (with `Times.Once` verifications), not-found, and invalid-state. Where a branch does not exist in the handler, fewer cases is correct — never add a test that asserts nothing just to reach the count.
**Rule 6.3 (mandatory):** assert outcomes, not internals — `result.IsSuccess`, state transitions, `Assert.Single`/`Assert.Empty`, captured aggregate contents. Never assert private implementation details.
**Rule 6.4 (recommended):** when a `DomainException` message is part of the contract (user-facing text), assert the message too; otherwise assert the type only.
**Rule 6.5:** coverage is collected with `--collect:"XPlat Code Coverage"`; the target is protection of behavior, not a percentage. Do not chase numbers with assertion-free tests.

## 7. Anti-patterns (review blockers)

- More than one Act or one behavior in a single test.
- `.Result` / `.Wait()` / `async void` in async tests.
- Mocking domain entities or value objects.
- Placeholder tests (`UnitTest1`, `Assert.True(true)`, empty bodies).
- Shared mutable fixtures or static mutable state across tests.
- Real I/O, network, `DateTime.Now`, or timezone dependence.
- Failure-path tests that never assert the absence of a commit.
- Tests that pass with the production logic deleted.

## 8. PR review checklist

- [ ] Suite green: `dotnet test src/Nutricenter.ContractsBilling.Test` → `0 failed`.
- [ ] File/namespace/method naming follows Rule 2.
- [ ] One Act per test; AAA readable top-to-bottom.
- [ ] Factories reused; no duplicated arrange blocks (Rule 4.1).
- [ ] Only ports mocked; domain objects real (Rules 5.1–5.2).
- [ ] Success verifies `Times.Once`; failure verifies `Times.Never` (Rules 5.3–5.4).
- [ ] Every new guard shipped with both branches (Rule 6.1).
- [ ] No new test framework added without updating this file (Rule 1.2).

## 9. Skeletons

Command handler (real pattern from `ContractCreateCommandHandlerTest`):

```csharp
[Fact]
public async Task Handle_WithServices_ShouldCreateContractAndCommit()
{
    // Arrange
    var repositoryMock = new Mock<IContractRepository>();
    var numberGeneratorMock = new Mock<IContractNumberGenerator>();
    var unitOfWorkMock = new Mock<IUnitOfWork>();
    numberGeneratorMock
        .Setup(x => x.GenerateAsync(It.IsAny<string>()))
        .ReturnsAsync(new SequenceNumberGenerator("CTR-2026-00001"));
    ContractAggregate? created = null;
    repositoryMock
        .Setup(x => x.CreateAsync(It.IsAny<ContractAggregate>()))
        .Callback<ContractAggregate>(c => created = c)
        .Returns(Task.CompletedTask);
    var handler = new ContractCreateCommandHandler(
        repositoryMock.Object, numberGeneratorMock.Object, unitOfWorkMock.Object);
    var command = CreateCommand();

    // Act
    var result = await handler.Handle(command, CancellationToken.None);

    // Assert
    Assert.True(result.IsSuccess);
    Assert.NotNull(created);
    Assert.Single(created.Services);
    repositoryMock.Verify(x => x.CreateAsync(It.IsAny<ContractAggregate>()), Times.Once);
    unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
}
```

Domain guard, both branches:

```csharp
[Fact]
public void Activate_EmptyDraftContract_ShouldThrowDomainException()
{
    var contract = CreateContract();

    Assert.Throws<DomainException>(() => contract.Activate());
}

[Fact]
public void Activate_ContractWithServices_ShouldBecomeActive()
{
    var contract = CreateContract();
    contract.AddService(CreateService());

    contract.Activate();

    Assert.Equal(ContractStatus.Active, contract.Estado);
}
```

---
Status: draft — pending team review
Runner: `dotnet test src/Nutricenter.ContractsBilling.Test`
Supersedes: nothing — complements `project/unit-test-rules.md`
