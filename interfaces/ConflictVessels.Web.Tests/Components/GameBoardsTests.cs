using Bunit;
using ConflictVessels.Engine.Grids;
using ConflictVessels.Engine.Vessels;
using ConflictVessels.Web.Components;

namespace ConflictVessels.Web.Tests.Components;

/// <summary>
/// Unit tests for the GameBoards component.
/// Tests verify rendering of multiple player boards and parameter handling.
/// </summary>
public class GameBoardsTests : TestContext
{
    #region Rendering Tests

    [Fact]
    public void Component_renders_without_errors()
    {
        // Arrange
        var vessels = new Vessel[] { new Vessel(2), new Vessel(3) };
        var grid1 = new AutoGrid(10, 10, vessels);
        var grid2 = new AutoGrid(10, 10, vessels);

        // Act
        var cut = RenderComponent<GameBoards>(parameters => parameters
            .Add(p => p.Player1Grid, grid1)
            .Add(p => p.Player2Grid, grid2));

        // Assert
        Assert.NotNull(cut);
    }

    [Fact]
    public void Component_renders_two_player_boards()
    {
        // Arrange
        var vessels = new Vessel[] { new Vessel(2), new Vessel(3) };
        var grid1 = new AutoGrid(10, 10, vessels);
        var grid2 = new AutoGrid(10, 10, vessels);

        // Act
        var cut = RenderComponent<GameBoards>(parameters => parameters
            .Add(p => p.Player1Grid, grid1)
            .Add(p => p.Player2Grid, grid2));

        // Assert
        var playerBoards = cut.FindAll(".player-board");
        Assert.Equal(2, playerBoards.Count);
    }

    [Fact]
    public void Component_renders_nothing_when_Player1Grid_is_null()
    {
        // Arrange
        var vessels = new Vessel[] { new Vessel(2), new Vessel(3) };
        var grid2 = new AutoGrid(10, 10, vessels);

        // Act
        var cut = RenderComponent<GameBoards>(parameters => parameters
            .Add(p => p.Player1Grid, null)
            .Add(p => p.Player2Grid, grid2));

        // Assert
        var playerBoards = cut.FindAll(".player-board");
        Assert.Empty(playerBoards);
    }

    [Fact]
    public void Component_renders_nothing_when_Player2Grid_is_null()
    {
        // Arrange
        var vessels = new Vessel[] { new Vessel(2), new Vessel(3) };
        var grid1 = new AutoGrid(10, 10, vessels);

        // Act
        var cut = RenderComponent<GameBoards>(parameters => parameters
            .Add(p => p.Player1Grid, grid1)
            .Add(p => p.Player2Grid, null));

        // Assert
        var playerBoards = cut.FindAll(".player-board");
        Assert.Empty(playerBoards);
    }

    [Fact]
    public void Component_renders_nothing_when_both_grids_are_null()
    {
        // Act
        var cut = RenderComponent<GameBoards>(parameters => parameters
            .Add(p => p.Player1Grid, null)
            .Add(p => p.Player2Grid, null));

        // Assert
        var playerBoards = cut.FindAll(".player-board");
        Assert.Empty(playerBoards);
    }

    #endregion

    #region Player Board Tests

    [Fact]
    public void Component_displays_Player_1_label()
    {
        // Arrange
        var vessels = new Vessel[] { new Vessel(2), new Vessel(3) };
        var grid1 = new AutoGrid(10, 10, vessels);
        var grid2 = new AutoGrid(10, 10, vessels);

        // Act
        var cut = RenderComponent<GameBoards>(parameters => parameters
            .Add(p => p.Player1Grid, grid1)
            .Add(p => p.Player2Grid, grid2));

        // Assert
        var headings = cut.FindAll("h2");
        Assert.Contains(headings, h => h.TextContent == "Player 1");
    }

    [Fact]
    public void Component_displays_Player_2_label()
    {
        // Arrange
        var vessels = new Vessel[] { new Vessel(2), new Vessel(3) };
        var grid1 = new AutoGrid(10, 10, vessels);
        var grid2 = new AutoGrid(10, 10, vessels);

        // Act
        var cut = RenderComponent<GameBoards>(parameters => parameters
            .Add(p => p.Player1Grid, grid1)
            .Add(p => p.Player2Grid, grid2));

        // Assert
        var headings = cut.FindAll("h2");
        Assert.Contains(headings, h => h.TextContent == "Player 2");
    }

    [Fact]
    public void Component_renders_grids_for_both_players()
    {
        // Arrange
        var vessels = new Vessel[] { new Vessel(2), new Vessel(3) };
        var grid1 = new AutoGrid(10, 10, vessels);
        var grid2 = new AutoGrid(10, 10, vessels);

        // Act
        var cut = RenderComponent<GameBoards>(parameters => parameters
            .Add(p => p.Player1Grid, grid1)
            .Add(p => p.Player2Grid, grid2));

        // Assert
        var grids = cut.FindAll(".game-grid");
        Assert.Equal(2, grids.Count);
    }

    #endregion

    #region Parameter Update Tests

    [Fact]
    public void Component_updates_when_grid_parameters_change()
    {
        // Arrange
        var vessels = new Vessel[] { new Vessel(2), new Vessel(3) };
        var grid1 = new AutoGrid(10, 10, vessels);
        var grid2 = new AutoGrid(10, 10, vessels);

        var cut = RenderComponent<GameBoards>(parameters => parameters
            .Add(p => p.Player1Grid, grid1)
            .Add(p => p.Player2Grid, grid2));

        var newGrid1 = new AutoGrid(10, 10, vessels);
        var newGrid2 = new AutoGrid(10, 10, vessels);

        // Act
        cut.SetParametersAndRender(parameters => parameters
            .Add(p => p.Player1Grid, newGrid1)
            .Add(p => p.Player2Grid, newGrid2));

        // Assert
        var playerBoards = cut.FindAll(".player-board");
        Assert.Equal(2, playerBoards.Count);
    }

    [Fact]
    public void Component_hides_boards_when_grids_change_to_null()
    {
        // Arrange
        var vessels = new Vessel[] { new Vessel(2), new Vessel(3) };
        var grid1 = new AutoGrid(10, 10, vessels);
        var grid2 = new AutoGrid(10, 10, vessels);

        var cut = RenderComponent<GameBoards>(parameters => parameters
            .Add(p => p.Player1Grid, grid1)
            .Add(p => p.Player2Grid, grid2));

        // Act
        cut.SetParametersAndRender(parameters => parameters
            .Add(p => p.Player1Grid, null)
            .Add(p => p.Player2Grid, null));

        // Assert
        var playerBoards = cut.FindAll(".player-board");
        Assert.Empty(playerBoards);
    }

    #endregion

    #region Component Structure Tests

    [Fact]
    public void Component_has_game_boards_container()
    {
        // Arrange
        var vessels = new Vessel[] { new Vessel(2), new Vessel(3) };
        var grid1 = new AutoGrid(10, 10, vessels);
        var grid2 = new AutoGrid(10, 10, vessels);

        // Act
        var cut = RenderComponent<GameBoards>(parameters => parameters
            .Add(p => p.Player1Grid, grid1)
            .Add(p => p.Player2Grid, grid2));

        // Assert
        var container = cut.Find(".game-boards");
        Assert.NotNull(container);
    }

    #endregion
}
