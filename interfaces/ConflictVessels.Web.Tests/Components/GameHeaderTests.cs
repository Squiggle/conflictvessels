using Bunit;
using ConflictVessels.Engine;
using ConflictVessels.Web.Components;
using Microsoft.AspNetCore.Components;

namespace ConflictVessels.Web.Tests.Components;

/// <summary>
/// Unit tests for the GameHeader component.
/// Tests verify rendering of phase indicators and button interactions.
/// </summary>
public class GameHeaderTests : TestContext
{
    #region Rendering Tests

    [Fact]
    public void Component_renders_without_errors()
    {
        // Act
        var cut = RenderComponent<GameHeader>(parameters => parameters
            .Add(p => p.Phase, GamePhase.Setup));

        // Assert
        Assert.NotNull(cut);
    }

    [Fact]
    public void Component_displays_phase_indicator()
    {
        // Act
        var cut = RenderComponent<GameHeader>(parameters => parameters
            .Add(p => p.Phase, GamePhase.Setup));

        // Assert
        var indicator = cut.Find(".phase-indicator");
        Assert.NotNull(indicator);
    }

    [Fact]
    public void Component_displays_correct_phase_value_for_Setup()
    {
        // Act
        var cut = RenderComponent<GameHeader>(parameters => parameters
            .Add(p => p.Phase, GamePhase.Setup));

        // Assert
        var indicator = cut.Find(".phase-indicator");
        Assert.Contains("Setup", indicator.TextContent);
    }

    [Fact]
    public void Component_displays_correct_phase_value_for_Action()
    {
        // Act
        var cut = RenderComponent<GameHeader>(parameters => parameters
            .Add(p => p.Phase, GamePhase.Action));

        // Assert
        var indicator = cut.Find(".phase-indicator");
        Assert.Contains("Action", indicator.TextContent);
    }

    [Fact]
    public void Component_displays_correct_phase_value_for_Ended()
    {
        // Act
        var cut = RenderComponent<GameHeader>(parameters => parameters
            .Add(p => p.Phase, GamePhase.Ended));

        // Assert
        var indicator = cut.Find(".phase-indicator");
        Assert.Contains("Ended", indicator.TextContent);
    }

    [Fact]
    public void Component_applies_phase_specific_css_class_for_Setup()
    {
        // Act
        var cut = RenderComponent<GameHeader>(parameters => parameters
            .Add(p => p.Phase, GamePhase.Setup));

        // Assert
        var indicator = cut.Find(".phase-indicator");
        Assert.Contains("phase-setup", indicator.ClassList);
    }

    [Fact]
    public void Component_applies_phase_specific_css_class_for_Action()
    {
        // Act
        var cut = RenderComponent<GameHeader>(parameters => parameters
            .Add(p => p.Phase, GamePhase.Action));

        // Assert
        var indicator = cut.Find(".phase-indicator");
        Assert.Contains("phase-action", indicator.ClassList);
    }

    [Fact]
    public void Component_applies_phase_specific_css_class_for_Ended()
    {
        // Act
        var cut = RenderComponent<GameHeader>(parameters => parameters
            .Add(p => p.Phase, GamePhase.Ended));

        // Assert
        var indicator = cut.Find(".phase-indicator");
        Assert.Contains("phase-ended", indicator.ClassList);
    }

    [Fact]
    public void Component_renders_new_game_button()
    {
        // Act
        var cut = RenderComponent<GameHeader>(parameters => parameters
            .Add(p => p.Phase, GamePhase.Setup));

        // Assert
        var button = cut.Find("button.btn-secondary");
        Assert.Equal("New Game", button.TextContent);
    }

    #endregion

    #region Button Interaction Tests

    [Fact]
    public void Clicking_new_game_button_invokes_callback()
    {
        // Arrange
        var callbackInvoked = false;
        var cut = RenderComponent<GameHeader>(parameters => parameters
            .Add(p => p.Phase, GamePhase.Setup)
            .Add(p => p.OnNewGame, EventCallback.Factory.Create(this, () => callbackInvoked = true)));

        var button = cut.Find("button.btn-secondary");

        // Act
        button.Click();

        // Assert
        Assert.True(callbackInvoked);
    }

    [Fact]
    public void New_game_button_does_not_throw_when_callback_not_set()
    {
        // Arrange
        var cut = RenderComponent<GameHeader>(parameters => parameters
            .Add(p => p.Phase, GamePhase.Setup));

        var button = cut.Find("button.btn-secondary");

        // Act & Assert - should not throw
        button.Click();
    }

    #endregion

    #region Parameter Update Tests

    [Fact]
    public void Phase_indicator_updates_when_parameter_changes()
    {
        // Arrange
        var cut = RenderComponent<GameHeader>(parameters => parameters
            .Add(p => p.Phase, GamePhase.Setup));

        // Act
        cut.SetParametersAndRender(parameters => parameters
            .Add(p => p.Phase, GamePhase.Action));

        // Assert
        var indicator = cut.Find(".phase-indicator");
        Assert.Contains("Action", indicator.TextContent);
    }

    [Fact]
    public void CSS_class_updates_when_phase_changes()
    {
        // Arrange
        var cut = RenderComponent<GameHeader>(parameters => parameters
            .Add(p => p.Phase, GamePhase.Setup));

        // Act
        cut.SetParametersAndRender(parameters => parameters
            .Add(p => p.Phase, GamePhase.Action));

        // Assert
        var indicator = cut.Find(".phase-indicator");
        Assert.Contains("phase-action", indicator.ClassList);
        Assert.DoesNotContain("phase-setup", indicator.ClassList);
    }

    #endregion

    #region Component Structure Tests

    [Fact]
    public void Component_has_game_info_container()
    {
        // Act
        var cut = RenderComponent<GameHeader>(parameters => parameters
            .Add(p => p.Phase, GamePhase.Setup));

        // Assert
        var container = cut.Find(".game-info");
        Assert.NotNull(container);
    }

    [Fact]
    public void Component_contains_both_indicator_and_button()
    {
        // Act
        var cut = RenderComponent<GameHeader>(parameters => parameters
            .Add(p => p.Phase, GamePhase.Setup));

        // Assert
        var indicator = cut.Find(".phase-indicator");
        var button = cut.Find("button.btn-secondary");
        Assert.NotNull(indicator);
        Assert.NotNull(button);
    }

    #endregion
}
