# ConflictVessels.Web.Tests

Unit tests for the ConflictVessels.Web Blazor interface using bUnit and xUnit.

## Test Coverage Requirements

As per project policy, all new code requires minimum 95% code coverage.

## Running Tests

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

# Or use make command from project root
make coverage
```

## Test Structure

Tests mirror the source structure:

```
ConflictVessels.Web.Tests/
├── Components/
│   ├── GridComponentTests.cs
│   └── Pages/
│       └── GameTests.cs
├── Pages/
│   ├── HomeTests.cs
├── Layout/
│   └── NavMenuTests.cs
└── Services/
    └── GameServiceTests.cs
```

## Testing with bUnit

bUnit is used for testing Blazor components. Key features:

- Render components in isolation
- Test component markup and behavior
- Simulate user interactions
- Verify event callbacks
- Mock dependencies and services

## Example Test Structure

```csharp
public class ComponentTests : TestContext
{
    [Fact]
    public void Component_renders_correctly()
    {
        // Arrange
        var cut = RenderComponent<YourComponent>(parameters => parameters
            .Add(p => p.SomeParameter, "value"));

        // Act & Assert
        cut.MarkupMatches("<expected markup>");
    }
}
```
