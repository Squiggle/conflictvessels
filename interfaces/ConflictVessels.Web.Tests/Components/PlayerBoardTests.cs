using Bunit;
using ConflictVessels.Engine.Grids;
using ConflictVessels.Engine.Vessels;
using ConflictVessels.Web.Components;

namespace ConflictVessels.Web.Tests.Components;

/// <summary>
/// Unit tests for the PlayerBoard component.
/// Tests verify rendering of player labels and grid components.
/// </summary>
public class PlayerBoardTests : TestContext
{
    #region Rendering Tests

    [Fact]
    public void Component_renders_without_errors()
    {
        // Arrange
        var vessels = new Vessel[] { new Vessel(2), new Vessel(3) };
        var grid = new AutoGrid(10, 10, vessels);

        // Act
        var cut = RenderComponent<PlayerBoard>(parameters => parameters
            .Add(p => p.PlayerNumber, 1)
            .Add(p => p.Grid, grid));

        // Assert
        Assert.NotNull(cut);
    }

    [Fact]
    public void Component_displays_player_number()
    {
        // Arrange
        var vessels = new Vessel[] { new Vessel(2), new Vessel(3) };
        var grid = new AutoGrid(10, 10, vessels);

        // Act
        var cut = RenderComponent<PlayerBoard>(parameters => parameters
            .Add(p => p.PlayerNumber, 1)
            .Add(p => p.Grid, grid));

        // Assert
        var heading = cut.Find("h2");
        Assert.Equal("Player 1", heading.TextContent);
    }

    [Fact]
    public void Component_displays_correct_player_number_for_player_2()
    {
        // Arrange
        var vessels = new Vessel[] { new Vessel(2), new Vessel(3) };
        var grid = new AutoGrid(10, 10, vessels);

        // Act
        var cut = RenderComponent<PlayerBoard>(parameters => parameters
            .Add(p => p.PlayerNumber, 2)
            .Add(p => p.Grid, grid));

        // Assert
        var heading = cut.Find("h2");
        Assert.Equal("Player 2", heading.TextContent);
    }

    [Fact]
    public void Component_renders_GridComponent()
    {
        // Arrange
        var vessels = new Vessel[] { new Vessel(2), new Vessel(3) };
        var grid = new AutoGrid(10, 10, vessels);

        // Act
        var cut = RenderComponent<PlayerBoard>(parameters => parameters
            .Add(p => p.PlayerNumber, 1)
            .Add(p => p.Grid, grid));

        // Assert
        var gridComponent = cut.Find(".game-grid");
        Assert.NotNull(gridComponent);
    }

    [Fact]
    public void Component_renders_grid_with_correct_cell_count()
    {
        // Arrange
        var vessels = new Vessel[] { new Vessel(2), new Vessel(3) };
        var grid = new AutoGrid(10, 10, vessels);

        // Act
        var cut = RenderComponent<PlayerBoard>(parameters => parameters
            .Add(p => p.PlayerNumber, 1)
            .Add(p => p.Grid, grid));

        // Assert
        var cells = cut.FindAll(".grid-cell");
        Assert.Equal(100, cells.Count); // 10x10 grid
    }

    #endregion

    #region Parameter Update Tests

    [Fact]
    public void Component_updates_when_PlayerNumber_changes()
    {
        // Arrange
        var vessels = new Vessel[] { new Vessel(2), new Vessel(3) };
        var grid = new AutoGrid(10, 10, vessels);

        var cut = RenderComponent<PlayerBoard>(parameters => parameters
            .Add(p => p.PlayerNumber, 1)
            .Add(p => p.Grid, grid));

        // Act
        cut.SetParametersAndRender(parameters => parameters
            .Add(p => p.PlayerNumber, 3)
            .Add(p => p.Grid, grid));

        // Assert
        var heading = cut.Find("h2");
        Assert.Equal("Player 3", heading.TextContent);
    }

    [Fact]
    public void Component_updates_when_Grid_changes()
    {
        // Arrange
        var vessels = new Vessel[] { new Vessel(2), new Vessel(3) };
        var grid1 = new AutoGrid(10, 10, vessels);
        var grid2 = new AutoGrid(10, 10, vessels);

        var cut = RenderComponent<PlayerBoard>(parameters => parameters
            .Add(p => p.PlayerNumber, 1)
            .Add(p => p.Grid, grid1));

        // Act
        cut.SetParametersAndRender(parameters => parameters
            .Add(p => p.PlayerNumber, 1)
            .Add(p => p.Grid, grid2));

        // Assert
        var gridComponent = cut.Find(".game-grid");
        Assert.NotNull(gridComponent);
    }

    #endregion

    #region Component Structure Tests

    [Fact]
    public void Component_has_player_board_container()
    {
        // Arrange
        var vessels = new Vessel[] { new Vessel(2), new Vessel(3) };
        var grid = new AutoGrid(10, 10, vessels);

        // Act
        var cut = RenderComponent<PlayerBoard>(parameters => parameters
            .Add(p => p.PlayerNumber, 1)
            .Add(p => p.Grid, grid));

        // Assert
        var container = cut.Find(".player-board");
        Assert.NotNull(container);
    }

    [Fact]
    public void Component_contains_both_heading_and_grid()
    {
        // Arrange
        var vessels = new Vessel[] { new Vessel(2), new Vessel(3) };
        var grid = new AutoGrid(10, 10, vessels);

        // Act
        var cut = RenderComponent<PlayerBoard>(parameters => parameters
            .Add(p => p.PlayerNumber, 1)
            .Add(p => p.Grid, grid));

        // Assert
        var heading = cut.Find("h2");
        var gridComponent = cut.Find(".game-grid");
        Assert.NotNull(heading);
        Assert.NotNull(gridComponent);
    }

    #endregion

    #region Edge Case Tests

    [Fact]
    public void Component_handles_zero_player_number()
    {
        // Arrange
        var vessels = new Vessel[] { new Vessel(2), new Vessel(3) };
        var grid = new AutoGrid(10, 10, vessels);

        // Act
        var cut = RenderComponent<PlayerBoard>(parameters => parameters
            .Add(p => p.PlayerNumber, 0)
            .Add(p => p.Grid, grid));

        // Assert
        var heading = cut.Find("h2");
        Assert.Equal("Player 0", heading.TextContent);
    }

    [Fact]
    public void Component_handles_large_player_number()
    {
        // Arrange
        var vessels = new Vessel[] { new Vessel(2), new Vessel(3) };
        var grid = new AutoGrid(10, 10, vessels);

        // Act
        var cut = RenderComponent<PlayerBoard>(parameters => parameters
            .Add(p => p.PlayerNumber, 99)
            .Add(p => p.Grid, grid));

        // Assert
        var heading = cut.Find("h2");
        Assert.Equal("Player 99", heading.TextContent);
    }

    #endregion
}
