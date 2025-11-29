using ConflictVessels.Engine;
using ConflictVessels.Web.Services;

namespace ConflictVessels.Web.Tests.Services;

/// <summary>
/// Unit tests for the GameService class.
/// Tests verify game lifecycle management, state changes, grid access, and proper disposal.
/// </summary>
public class GameServiceTests
{
    #region Constructor and Initialization Tests

    [Fact]
    public void Constructor_initializes_with_null_game()
    {
        // Arrange & Act
        var service = new GameService();

        // Assert
        Assert.Null(service.CurrentGame);
    }

    [Fact]
    public void CurrentGame_returns_null_initially()
    {
        // Arrange
        var service = new GameService();

        // Act
        var game = service.CurrentGame;

        // Assert
        Assert.Null(game);
    }

    #endregion

    #region StartNewGame Method Tests

    [Fact]
    public void StartNewGame_creates_new_game_instance()
    {
        // Arrange
        var service = new GameService();

        // Act
        service.StartNewGame();

        // Assert
        Assert.NotNull(service.CurrentGame);
    }

    [Fact]
    public void StartNewGame_creates_game_with_two_players()
    {
        // Arrange
        var service = new GameService();

        // Act
        service.StartNewGame();

        // Assert
        Assert.NotNull(service.CurrentGame);
        Assert.Equal(2, service.CurrentGame.Players.Count);
    }

    [Fact]
    public void StartNewGame_creates_arena_with_two_grids()
    {
        // Arrange
        var service = new GameService();

        // Act
        service.StartNewGame();

        // Assert
        Assert.NotNull(service.CurrentGame);
        Assert.Equal(2, service.CurrentGame.Arena.Grids.Count);
    }

    [Fact]
    public void StartNewGame_creates_grids_with_correct_dimensions()
    {
        // Arrange
        var service = new GameService();

        // Act
        service.StartNewGame();

        // Assert
        Assert.NotNull(service.CurrentGame);
        var grid1 = service.CurrentGame.Arena.Grids[0];
        var grid2 = service.CurrentGame.Arena.Grids[1];

        Assert.Equal(10, grid1.Width);
        Assert.Equal(10, grid1.Height);
        Assert.Equal(10, grid2.Width);
        Assert.Equal(10, grid2.Height);
    }

    [Fact]
    public void StartNewGame_creates_grids_with_five_vessels_each()
    {
        // Arrange
        var service = new GameService();

        // Act
        service.StartNewGame();

        // Assert
        Assert.NotNull(service.CurrentGame);
        var grid1 = service.CurrentGame.Arena.Grids[0];
        var grid2 = service.CurrentGame.Arena.Grids[1];

        Assert.Equal(5, grid1.Vessels.Count);
        Assert.Equal(5, grid2.Vessels.Count);
    }

    [Fact]
    public void StartNewGame_creates_vessels_with_correct_sizes()
    {
        // Arrange
        var service = new GameService();

        // Act
        service.StartNewGame();

        // Assert
        Assert.NotNull(service.CurrentGame);
        var grid = service.CurrentGame.Arena.Grids[0];
        var vesselSizes = grid.Vessels.Select(v => v.Vessel.Size).OrderBy(s => s).ToList();

        Assert.Equal(new[] { 2, 3, 3, 4, 5 }, vesselSizes);
    }

    [Fact]
    public void StartNewGame_exposes_game_with_observable_phase()
    {
        // Arrange
        var service = new GameService();

        // Act
        service.StartNewGame();

        // Assert
        Assert.NotNull(service.CurrentGame);
        Assert.NotNull(service.CurrentGame.ObservablePhase);
    }

    [Fact]
    public void StartNewGame_disposes_previous_game_when_called_multiple_times()
    {
        // Arrange
        var service = new GameService();
        service.StartNewGame();
        var firstGame = service.CurrentGame;

        // Act
        service.StartNewGame();
        var secondGame = service.CurrentGame;

        // Assert
        Assert.NotNull(firstGame);
        Assert.NotNull(secondGame);
        Assert.NotSame(firstGame, secondGame);
    }

    [Fact]
    public void StartNewGame_replaces_previous_game_instance()
    {
        // Arrange
        var service = new GameService();
        service.StartNewGame();
        var firstGame = service.CurrentGame;

        // Act
        service.StartNewGame();

        // Assert
        Assert.NotNull(firstGame);
        Assert.NotNull(service.CurrentGame);
        Assert.NotSame(firstGame, service.CurrentGame);
    }

    [Fact]
    public void CurrentGame_allows_direct_subscription_to_phase_observable()
    {
        // Arrange
        var service = new GameService();
        service.StartNewGame();
        GamePhase? observedPhase = null;

        // Act
        using var subscription = service.CurrentGame?.ObservablePhase
            .Subscribe(phase => observedPhase = phase);

        // Assert
        Assert.NotNull(service.CurrentGame);
        Assert.Equal(service.CurrentGame.Phase, observedPhase);
    }

    #endregion

    #region GetPlayerGrid Method Tests

    [Fact]
    public void GetPlayerGrid_returns_null_when_game_is_null()
    {
        // Arrange
        var service = new GameService();

        // Act
        var grid = service.GetPlayerGrid(0);

        // Assert
        Assert.Null(grid);
    }

    [Fact]
    public void GetPlayerGrid_returns_null_for_negative_index()
    {
        // Arrange
        var service = new GameService();
        service.StartNewGame();

        // Act
        var grid = service.GetPlayerGrid(-1);

        // Assert
        Assert.Null(grid);
    }

    [Fact]
    public void GetPlayerGrid_returns_null_for_index_exceeding_grid_count()
    {
        // Arrange
        var service = new GameService();
        service.StartNewGame();

        // Act
        var grid = service.GetPlayerGrid(2);

        // Assert
        Assert.Null(grid);
    }

    [Fact]
    public void GetPlayerGrid_returns_correct_grid_for_player_0()
    {
        // Arrange
        var service = new GameService();
        service.StartNewGame();

        // Act
        var grid = service.GetPlayerGrid(0);

        // Assert
        Assert.NotNull(grid);
        Assert.Same(service.CurrentGame!.Arena.Grids[0], grid);
    }

    [Fact]
    public void GetPlayerGrid_returns_correct_grid_for_player_1()
    {
        // Arrange
        var service = new GameService();
        service.StartNewGame();

        // Act
        var grid = service.GetPlayerGrid(1);

        // Assert
        Assert.NotNull(grid);
        Assert.Same(service.CurrentGame!.Arena.Grids[1], grid);
    }

    [Fact]
    public void GetPlayerGrid_returns_different_grids_for_different_players()
    {
        // Arrange
        var service = new GameService();
        service.StartNewGame();

        // Act
        var grid0 = service.GetPlayerGrid(0);
        var grid1 = service.GetPlayerGrid(1);

        // Assert
        Assert.NotNull(grid0);
        Assert.NotNull(grid1);
        Assert.NotSame(grid0, grid1);
    }

    #endregion

    #region Dispose Method Tests

    [Fact]
    public void Dispose_completes_without_errors()
    {
        // Arrange
        var service = new GameService();
        service.StartNewGame();

        // Act
        service.Dispose();

        // Assert
        Assert.NotNull(service); // Service object still exists but is disposed
    }

    [Fact]
    public void Dispose_disposes_current_game()
    {
        // Arrange
        var service = new GameService();
        service.StartNewGame();

        // Act
        service.Dispose();

        // Assert
        Assert.Null(service.CurrentGame);
    }

    [Fact]
    public void Dispose_sets_game_to_null()
    {
        // Arrange
        var service = new GameService();
        service.StartNewGame();
        Assert.NotNull(service.CurrentGame);

        // Act
        service.Dispose();

        // Assert
        Assert.Null(service.CurrentGame);
    }

    [Fact]
    public void Dispose_can_be_called_multiple_times_safely()
    {
        // Arrange
        var service = new GameService();
        service.StartNewGame();

        // Act & Assert - should not throw
        service.Dispose();
        service.Dispose();
        service.Dispose();

        Assert.Null(service.CurrentGame);
    }

    [Fact]
    public void Dispose_disposes_game_but_local_references_remain_valid()
    {
        // Arrange
        var service = new GameService();
        service.StartNewGame();
        var game = service.CurrentGame;

        // Act
        service.Dispose();

        // Assert
        Assert.Null(service.CurrentGame);
        Assert.NotNull(game); // Original game reference still exists
    }

    #endregion
}
