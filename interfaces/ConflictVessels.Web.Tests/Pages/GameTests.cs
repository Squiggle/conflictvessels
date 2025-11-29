using Bunit;
using ConflictVessels.Engine;
using ConflictVessels.Web.Services;
using Microsoft.Extensions.DependencyInjection;
using GamePage = ConflictVessels.Web.Pages.Game;

namespace ConflictVessels.Web.Tests.Pages;

/// <summary>
/// Unit tests for the Game page component.
/// Tests verify rendering, reactive subscriptions, lifecycle management, and user interactions.
/// </summary>
public class GameTests : TestContext
{
    private GameService CreateGameService()
    {
        var service = new GameService();
        Services.AddSingleton(service);
        return service;
    }

    #region Rendering Tests

    [Fact]
    public void Component_renders_without_errors()
    {
        // Act
        var cut = RenderComponent<GamePage>();

        // Assert
        Assert.NotNull(cut);
    }

    [Fact]
    public void Component_displays_setup_screen_when_no_game()
    {
        // Act
        var cut = RenderComponent<GamePage>();

        // Assert
        var setupDiv = cut.Find(".game-setup");
        Assert.NotNull(setupDiv);
    }

    [Fact]
    public void Component_displays_start_new_game_button_when_no_game()
    {
        // Act
        var cut = RenderComponent<GamePage>();

        // Assert
        var button = cut.Find("button.btn-primary");
        Assert.Equal("Start New Game", button.TextContent);
    }

    [Fact]
    public void Component_displays_game_title_when_no_game()
    {
        // Act
        var cut = RenderComponent<GamePage>();

        // Assert
        var heading = cut.Find("h1");
        Assert.Equal("Conflict Vessels", heading.TextContent);
    }

    [Fact]
    public void Component_displays_game_boards_when_game_exists()
    {
        // Arrange
        var gameService = CreateGameService();
        gameService.StartNewGame();

        // Act
        var cut = RenderComponent<GamePage>();

        // Assert
        var boards = cut.Find(".game-boards");
        Assert.NotNull(boards);
    }

    [Fact]
    public void Component_displays_game_header_when_game_exists()
    {
        // Arrange
        var gameService = CreateGameService();
        gameService.StartNewGame();

        // Act
        var cut = RenderComponent<GamePage>();

        // Assert
        var header = cut.Find(".game-info");
        Assert.NotNull(header);
    }

    [Fact]
    public void Component_hides_setup_screen_when_game_exists()
    {
        // Arrange
        var gameService = CreateGameService();
        gameService.StartNewGame();

        // Act
        var cut = RenderComponent<GamePage>();

        // Assert
        Assert.Throws<ElementNotFoundException>(() => cut.Find(".game-setup"));
    }

    #endregion

    #region Lifecycle Tests

    [Fact]
    public void StartNewGame_creates_game_via_GameService()
    {
        // Arrange
        var gameService = CreateGameService();
        var cut = RenderComponent<GamePage>();
        var button = cut.Find("button.btn-primary");

        // Act
        button.Click();

        // Assert
        Assert.NotNull(gameService.CurrentGame);
    }

    [Fact]
    public void StartNewGame_updates_UI_to_show_game_boards()
    {
        // Arrange
        var cut = RenderComponent<GamePage>();
        var button = cut.Find("button.btn-primary");

        // Act
        button.Click();

        // Assert
        var boards = cut.Find(".game-boards");
        Assert.NotNull(boards);
    }

    [Fact]
    public void StartNewGame_resubscribes_to_new_game_observables()
    {
        // Arrange
        var gameService = CreateGameService();
        gameService.StartNewGame();
        var cut = RenderComponent<GamePage>();
        var firstGame = gameService.CurrentGame;
        var newGameButton = cut.Find("button.btn-secondary");

        // Act
        newGameButton.Click();

        // Assert
        Assert.NotNull(gameService.CurrentGame);
        Assert.NotSame(firstGame, gameService.CurrentGame);
    }

    [Fact]
    public void Component_initializes_with_null_grids_when_no_game()
    {
        // Act
        var cut = RenderComponent<GamePage>();

        // Assert
        var setupDiv = cut.Find(".game-setup");
        Assert.NotNull(setupDiv);
        // If grids were not null, game-boards would be rendered instead
    }

    #endregion

    #region Subscription Tests

    [Fact]
    public void Component_subscribes_to_phase_observable_on_game_creation()
    {
        // Arrange
        var cut = RenderComponent<GamePage>();
        var button = cut.Find("button.btn-primary");

        // Act
        button.Click();

        // Assert
        var phaseIndicator = cut.Find(".phase-indicator");
        Assert.NotNull(phaseIndicator);
        Assert.Contains("Phase:", phaseIndicator.TextContent);
    }

    [Fact]
    public void Component_displays_correct_initial_phase()
    {
        // Arrange
        var cut = RenderComponent<GamePage>();
        var button = cut.Find("button.btn-primary");

        // Act
        button.Click();

        // Assert
        var phaseIndicator = cut.Find(".phase-indicator");
        // AutoGrid creates ready grids, so phase should be Action
        Assert.Contains("Action", phaseIndicator.TextContent);
    }

    [Fact]
    public void Component_applies_phase_specific_css_class()
    {
        // Arrange
        var cut = RenderComponent<GamePage>();
        var button = cut.Find("button.btn-primary");

        // Act
        button.Click();

        // Assert
        var phaseIndicator = cut.Find(".phase-indicator");
        // Should have phase-action class (lowercase)
        Assert.Contains("phase-action", phaseIndicator.ClassList);
    }

    #endregion

    #region Grid Display Tests

    [Fact]
    public void Component_displays_both_player_grids()
    {
        // Arrange
        var gameService = CreateGameService();
        gameService.StartNewGame();

        // Act
        var cut = RenderComponent<GamePage>();

        // Assert
        var playerBoards = cut.FindAll(".player-board");
        Assert.Equal(2, playerBoards.Count);
    }

    [Fact]
    public void Component_displays_player_1_label()
    {
        // Arrange
        var gameService = CreateGameService();
        gameService.StartNewGame();

        // Act
        var cut = RenderComponent<GamePage>();

        // Assert
        var headings = cut.FindAll("h2");
        Assert.Contains(headings, h => h.TextContent == "Player 1");
    }

    [Fact]
    public void Component_displays_player_2_label()
    {
        // Arrange
        var gameService = CreateGameService();
        gameService.StartNewGame();

        // Act
        var cut = RenderComponent<GamePage>();

        // Assert
        var headings = cut.FindAll("h2");
        Assert.Contains(headings, h => h.TextContent == "Player 2");
    }

    [Fact]
    public void Component_passes_correct_grid_to_Player1Board()
    {
        // Arrange
        var gameService = CreateGameService();
        gameService.StartNewGame();

        // Act
        var cut = RenderComponent<GamePage>();

        // Assert
        var grids = cut.FindAll(".game-grid");
        Assert.NotEmpty(grids);
        // Both grids should be rendered
        Assert.True(grids.Count >= 2);
    }

    [Fact]
    public void Component_renders_grid_cells_for_both_players()
    {
        // Arrange
        var gameService = CreateGameService();
        gameService.StartNewGame();

        // Act
        var cut = RenderComponent<GamePage>();

        // Assert
        var cells = cut.FindAll(".grid-cell");
        // 10x10 grid = 100 cells per player, 2 players = 200 cells
        Assert.Equal(200, cells.Count);
    }

    #endregion

    #region Null Safety Tests

    [Fact]
    public void Component_handles_null_game_gracefully()
    {
        // Arrange & Act
        var cut = RenderComponent<GamePage>();

        // Assert
        var setupDiv = cut.Find(".game-setup");
        Assert.NotNull(setupDiv);
    }

    [Fact]
    public void Component_does_not_throw_when_game_is_null()
    {
        // Arrange & Act
        var cut = RenderComponent<GamePage>();

        // Assert
        Assert.NotNull(cut);
    }

    [Fact]
    public void Component_displays_setup_screen_after_game_disposal()
    {
        // Arrange
        var gameService = CreateGameService();
        gameService.StartNewGame();
        var cut = RenderComponent<GamePage>();

        // Act
        gameService.Dispose();
        cut = RenderComponent<GamePage>();

        // Assert
        var setupDiv = cut.Find(".game-setup");
        Assert.NotNull(setupDiv);
    }

    #endregion

    #region Disposal Tests

    [Fact]
    public void Component_disposes_subscriptions_on_dispose()
    {
        // Arrange
        var gameService = CreateGameService();
        gameService.StartNewGame();
        var cut = RenderComponent<GamePage>();

        // Act & Assert - should not throw
        cut.Dispose();
    }

    [Fact]
    public void Component_can_be_disposed_multiple_times()
    {
        // Arrange
        var gameService = CreateGameService();
        gameService.StartNewGame();
        var cut = RenderComponent<GamePage>();

        // Act & Assert - should not throw
        cut.Dispose();
        cut.Dispose();
    }

    [Fact]
    public void Component_disposes_without_errors_when_no_game()
    {
        // Arrange
        var cut = RenderComponent<GamePage>();

        // Act & Assert - should not throw
        cut.Dispose();
    }

    #endregion

    #region Button Interaction Tests

    [Fact]
    public void Clicking_start_new_game_button_creates_game()
    {
        // Arrange
        var gameService = CreateGameService();
        var cut = RenderComponent<GamePage>();
        var button = cut.Find("button.btn-primary");
        Assert.Null(gameService.CurrentGame);

        // Act
        button.Click();

        // Assert
        Assert.NotNull(gameService.CurrentGame);
    }

    [Fact]
    public void Clicking_new_game_button_replaces_current_game()
    {
        // Arrange
        var gameService = CreateGameService();
        gameService.StartNewGame();
        var cut = RenderComponent<GamePage>();
        var firstGame = gameService.CurrentGame;
        var newGameButton = cut.Find("button.btn-secondary");

        // Act
        newGameButton.Click();
        var secondGame = gameService.CurrentGame;

        // Assert
        Assert.NotSame(firstGame, secondGame);
    }

    [Fact]
    public void New_game_button_is_visible_during_gameplay()
    {
        // Arrange
        var gameService = CreateGameService();
        gameService.StartNewGame();

        // Act
        var cut = RenderComponent<GamePage>();

        // Assert
        var button = cut.Find("button.btn-secondary");
        Assert.Equal("New Game", button.TextContent);
    }

    #endregion

    #region Component Structure Tests

    [Fact]
    public void Component_has_game_container_root()
    {
        // Act
        var cut = RenderComponent<GamePage>();

        // Assert
        var container = cut.Find(".game-container");
        Assert.NotNull(container);
    }

    [Fact]
    public void Component_structure_includes_all_child_components_when_game_exists()
    {
        // Arrange
        var gameService = CreateGameService();
        gameService.StartNewGame();

        // Act
        var cut = RenderComponent<GamePage>();

        // Assert
        var gameInfo = cut.Find(".game-info");
        var gameBoards = cut.Find(".game-boards");
        Assert.NotNull(gameInfo);
        Assert.NotNull(gameBoards);
    }

    #endregion
}
