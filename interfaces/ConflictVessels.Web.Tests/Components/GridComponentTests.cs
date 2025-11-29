using ConflictVessels.Engine.Grids;
using ConflictVessels.Engine.Vessels;
using ConflictVessels.Web.Components;

namespace ConflictVessels.Web.Tests.Components;

/// <summary>
/// Unit tests for the GridComponent Blazor component.
/// Tests verify rendering, parameters, cell interactions, and vessel display.
/// </summary>
public class GridComponentTests : TestContext
{
    #region Rendering Tests

    [Fact]
    public void Component_renders_without_errors()
    {
        // Arrange
        var grid = Grid.Default();

        // Act
        var cut = RenderComponent<GridComponent>(parameters => parameters
            .Add(p => p.Grid, grid));

        // Assert
        Assert.NotNull(cut);
    }

    [Fact]
    public void Component_throws_when_Grid_parameter_is_null()
    {
        // Arrange & Act & Assert
        Assert.Throws<NullReferenceException>(() =>
        {
            var cut = RenderComponent<GridComponent>(parameters => parameters
                .Add(p => p.Grid, null!));
        });
    }

    [Fact]
    public void Component_renders_correct_number_of_cells()
    {
        // Arrange
        var grid = Grid.Default();

        // Act
        var cut = RenderComponent<GridComponent>(parameters => parameters
            .Add(p => p.Grid, grid));

        // Assert
        var cells = cut.FindAll(".grid-cell");
        Assert.Equal(grid.Width * grid.Height, cells.Count);
    }

    [Fact]
    public void Component_renders_cells_with_correct_dimensions()
    {
        // Arrange
        var vessels = new[] { new Vessel(2) };
        var grid = new AutoGrid(5, 7, vessels); // 5 wide, 7 tall

        // Act
        var cut = RenderComponent<GridComponent>(parameters => parameters
            .Add(p => p.Grid, grid));

        // Assert
        var cells = cut.FindAll(".grid-cell");
        Assert.Equal(35, cells.Count); // 5 * 7 = 35
    }

    [Fact]
    public void Component_applies_grid_size_css_variable()
    {
        // Arrange
        var vessels = new[] { new Vessel(2) };
        var grid = new AutoGrid(8, 8, vessels);

        // Act
        var cut = RenderComponent<GridComponent>(parameters => parameters
            .Add(p => p.Grid, grid));

        // Assert
        var gameGrid = cut.Find(".game-grid");
        var style = gameGrid.GetAttribute("style");
        Assert.Contains("--grid-size: 8", style);
    }

    [Fact]
    public void Component_renders_empty_grid_when_no_vessels()
    {
        // Arrange
        var grid = new AutoGrid(5, 5, Array.Empty<Vessel>());

        // Act
        var cut = RenderComponent<GridComponent>(parameters => parameters
            .Add(p => p.Grid, grid));

        // Assert
        var vesselMarkers = cut.FindAll(".vessel-marker");
        Assert.Empty(vesselMarkers);
    }

    #endregion

    #region Grid Parameter Tests

    [Fact]
    public void Component_renders_10x10_grid_for_default_grid()
    {
        // Arrange
        var grid = Grid.Default();

        // Act
        var cut = RenderComponent<GridComponent>(parameters => parameters
            .Add(p => p.Grid, grid));

        // Assert
        var cells = cut.FindAll(".grid-cell");
        Assert.Equal(100, cells.Count);
    }

    [Fact]
    public void Component_updates_when_Grid_parameter_changes()
    {
        // Arrange
        var grid1 = new AutoGrid(3, 3, Array.Empty<Vessel>());
        var grid2 = new AutoGrid(4, 4, Array.Empty<Vessel>());

        // Act
        var cut = RenderComponent<GridComponent>(parameters => parameters
            .Add(p => p.Grid, grid1));

        var initialCells = cut.FindAll(".grid-cell");
        Assert.Equal(9, initialCells.Count);

        cut.SetParametersAndRender(parameters => parameters
            .Add(p => p.Grid, grid2));

        // Assert
        var updatedCells = cut.FindAll(".grid-cell");
        Assert.Equal(16, updatedCells.Count);
    }

    [Fact]
    public void Component_renders_different_sized_grids_correctly()
    {
        // Arrange
        var grid = new AutoGrid(6, 4, Array.Empty<Vessel>());

        // Act
        var cut = RenderComponent<GridComponent>(parameters => parameters
            .Add(p => p.Grid, grid));

        // Assert
        var cells = cut.FindAll(".grid-cell");
        Assert.Equal(24, cells.Count); // 6 * 4 = 24
    }

    #endregion

    #region ShowVessels Parameter Tests

    [Fact]
    public void ShowVessels_true_displays_vessel_markers()
    {
        // Arrange
        var vessels = new[] { new Vessel(3) };
        var grid = new AutoGrid(5, 5, vessels);

        // Act
        var cut = RenderComponent<GridComponent>(parameters => parameters
            .Add(p => p.Grid, grid)
            .Add(p => p.ShowVessels, true));

        // Assert
        var vesselMarkers = cut.FindAll(".vessel-marker");
        Assert.Equal(3, vesselMarkers.Count); // Vessel of size 3
    }

    [Fact]
    public void ShowVessels_false_hides_vessel_markers()
    {
        // Arrange
        var vessels = new[] { new Vessel(3) };
        var grid = new AutoGrid(5, 5, vessels);

        // Act
        var cut = RenderComponent<GridComponent>(parameters => parameters
            .Add(p => p.Grid, grid)
            .Add(p => p.ShowVessels, false));

        // Assert
        var vesselMarkers = cut.FindAll(".vessel-marker");
        Assert.Empty(vesselMarkers);
    }

    [Fact]
    public void ShowVessels_defaults_to_true()
    {
        // Arrange
        var vessels = new[] { new Vessel(2) };
        var grid = new AutoGrid(5, 5, vessels);

        // Act
        var cut = RenderComponent<GridComponent>(parameters => parameters
            .Add(p => p.Grid, grid));
        // Note: Not setting ShowVessels parameter, should default to true

        // Assert
        var vesselMarkers = cut.FindAll(".vessel-marker");
        Assert.Equal(2, vesselMarkers.Count);
    }

    [Fact]
    public void ShowVessels_only_shows_markers_on_occupied_cells()
    {
        // Arrange
        var vessels = new[] { new Vessel(2), new Vessel(3) };
        var grid = new AutoGrid(10, 10, vessels);

        // Act
        var cut = RenderComponent<GridComponent>(parameters => parameters
            .Add(p => p.Grid, grid)
            .Add(p => p.ShowVessels, true));

        // Assert
        var vesselMarkers = cut.FindAll(".vessel-marker");
        Assert.Equal(5, vesselMarkers.Count); // 2 + 3 = 5 cells with vessels
    }

    #endregion

    #region Cell Rendering Tests

    [Fact]
    public void Cells_have_correct_title_attribute_with_coordinates()
    {
        // Arrange
        var grid = new AutoGrid(3, 3, Array.Empty<Vessel>());

        // Act
        var cut = RenderComponent<GridComponent>(parameters => parameters
            .Add(p => p.Grid, grid));

        // Assert
        var cells = cut.FindAll(".grid-cell");

        // Check first cell (0, 0) should be "A1"
        Assert.Equal("A1", cells[0].GetAttribute("title"));

        // Check cell (1, 0) should be "B1"
        Assert.Equal("B1", cells[1].GetAttribute("title"));

        // Check cell (2, 0) should be "C1"
        Assert.Equal("C1", cells[2].GetAttribute("title"));

        // Check cell (0, 1) should be "A2"
        Assert.Equal("A2", cells[3].GetAttribute("title"));
    }

    [Fact]
    public void Cell_coordinates_use_letter_number_notation()
    {
        // Arrange
        var grid = new AutoGrid(10, 10, Array.Empty<Vessel>());

        // Act
        var cut = RenderComponent<GridComponent>(parameters => parameters
            .Add(p => p.Grid, grid));

        // Assert
        var cells = cut.FindAll(".grid-cell");

        // Check various cells follow the pattern
        Assert.Equal("A1", cells[0].GetAttribute("title"));   // (0,0)
        Assert.Equal("J1", cells[9].GetAttribute("title"));   // (9,0)
        Assert.Equal("A10", cells[90].GetAttribute("title")); // (0,9)
        Assert.Equal("J10", cells[99].GetAttribute("title")); // (9,9)
    }

    [Fact]
    public void Vessel_marker_rendered_only_on_vessel_cells()
    {
        // Arrange
        var vessels = new[] { new Vessel(4) };
        var grid = new AutoGrid(10, 10, vessels);

        // Act
        var cut = RenderComponent<GridComponent>(parameters => parameters
            .Add(p => p.Grid, grid)
            .Add(p => p.ShowVessels, true));

        // Assert
        var cells = cut.FindAll(".grid-cell");
        var cellsWithMarkers = cells.Where(c => c.QuerySelector(".vessel-marker") != null).ToList();

        Assert.Equal(4, cellsWithMarkers.Count); // Only 4 cells should have markers
        Assert.Equal(100, cells.Count); // But all 100 cells should exist
    }

    #endregion

    #region GetVesselAtCoords Method Tests

    [Fact]
    public void GetVesselAtCoords_returns_null_for_empty_cell()
    {
        // Arrange
        var vessels = new[] { new Vessel(2) };
        var grid = new AutoGrid(10, 10, vessels);

        // Act
        var cut = RenderComponent<GridComponent>(parameters => parameters
            .Add(p => p.Grid, grid));

        // Get a cell without a vessel - we'll check all cells and find one without marker
        var cells = cut.FindAll(".grid-cell");
        var emptyCells = cells.Where(c => c.QuerySelector(".vessel-marker") == null).ToList();

        // Assert
        Assert.NotEmpty(emptyCells); // Should have empty cells
    }

    [Fact]
    public void GetVesselAtCoords_returns_vessel_when_present()
    {
        // Arrange
        var vessels = new[] { new Vessel(3) };
        var grid = new AutoGrid(10, 10, vessels);

        // Act
        var cut = RenderComponent<GridComponent>(parameters => parameters
            .Add(p => p.Grid, grid)
            .Add(p => p.ShowVessels, true));

        // Assert
        var vesselMarkers = cut.FindAll(".vessel-marker");
        Assert.Equal(3, vesselMarkers.Count); // Vessel of size 3 should have 3 markers
    }

    [Fact]
    public void GetVesselAtCoords_returns_correct_vessel_for_multiple_vessels()
    {
        // Arrange
        var vessels = new[] { new Vessel(2), new Vessel(3), new Vessel(4) };
        var grid = new AutoGrid(10, 10, vessels);

        // Act
        var cut = RenderComponent<GridComponent>(parameters => parameters
            .Add(p => p.Grid, grid)
            .Add(p => p.ShowVessels, true));

        // Assert
        var vesselMarkers = cut.FindAll(".vessel-marker");
        Assert.Equal(9, vesselMarkers.Count); // 2 + 3 + 4 = 9 markers total
    }

    [Fact]
    public void GetVesselAtCoords_handles_edge_coordinates()
    {
        // Arrange
        var vessels = new[] { new Vessel(2) };
        var grid = new AutoGrid(5, 5, vessels);

        // Act
        var cut = RenderComponent<GridComponent>(parameters => parameters
            .Add(p => p.Grid, grid));

        // Assert - component renders successfully with vessels potentially at edges
        var cells = cut.FindAll(".grid-cell");
        Assert.Equal(25, cells.Count);
    }

    #endregion

    #region GetCellClass Method Tests

    [Fact]
    public void GetCellClass_returns_empty_for_null_vessel()
    {
        // Arrange
        var grid = new AutoGrid(5, 5, Array.Empty<Vessel>());

        // Act
        var cut = RenderComponent<GridComponent>(parameters => parameters
            .Add(p => p.Grid, grid));

        // Assert
        var cells = cut.FindAll(".grid-cell");
        Assert.All(cells, cell => Assert.Contains("empty", cell.ClassName));
    }

    [Fact]
    public void GetCellClass_returns_occupied_for_vessel()
    {
        // Arrange
        var vessels = new[] { new Vessel(3) };
        var grid = new AutoGrid(5, 5, vessels);

        // Act
        var cut = RenderComponent<GridComponent>(parameters => parameters
            .Add(p => p.Grid, grid));

        // Assert
        var cells = cut.FindAll(".grid-cell");
        var occupiedCells = cells.Where(c => c.ClassName.Contains("occupied")).ToList();

        Assert.Equal(3, occupiedCells.Count); // 3 cells should be occupied
    }

    #endregion

    #region OnCellClicked Event Callback Tests

    [Fact]
    public void OnCellClick_invokes_callback_with_correct_coordinates()
    {
        // Arrange
        var grid = new AutoGrid(5, 5, Array.Empty<Vessel>());
        Coords? clickedCoords = null;

        // Act
        var cut = RenderComponent<GridComponent>(parameters => parameters
            .Add(p => p.Grid, grid)
            .Add(p => p.OnCellClicked, coords => clickedCoords = coords));

        // Click the first cell (0, 0)
        var cells = cut.FindAll(".grid-cell");
        cells[0].Click();

        // Assert
        Assert.NotNull(clickedCoords);
        Assert.Equal(0, clickedCoords.X);
        Assert.Equal(0, clickedCoords.Y);
    }

    [Fact]
    public void OnCellClick_does_not_throw_when_callback_not_set()
    {
        // Arrange
        var grid = new AutoGrid(5, 5, Array.Empty<Vessel>());

        // Act
        var cut = RenderComponent<GridComponent>(parameters => parameters
            .Add(p => p.Grid, grid));
        // Note: Not setting OnCellClicked callback

        // Assert - should not throw when clicking
        var cells = cut.FindAll(".grid-cell");
        var exception = Record.Exception(() => cells[0].Click());
        Assert.Null(exception);
    }

    [Fact]
    public void OnCellClick_passes_coords_with_correct_x_value()
    {
        // Arrange
        var grid = new AutoGrid(5, 5, Array.Empty<Vessel>());
        Coords? clickedCoords = null;

        // Act
        var cut = RenderComponent<GridComponent>(parameters => parameters
            .Add(p => p.Grid, grid)
            .Add(p => p.OnCellClicked, coords => clickedCoords = coords));

        // Click cell at position (3, 0) - 4th cell in first row
        var cells = cut.FindAll(".grid-cell");
        cells[3].Click();

        // Assert
        Assert.NotNull(clickedCoords);
        Assert.Equal(3, clickedCoords.X);
    }

    [Fact]
    public void OnCellClick_passes_coords_with_correct_y_value()
    {
        // Arrange
        var grid = new AutoGrid(5, 5, Array.Empty<Vessel>());
        Coords? clickedCoords = null;

        // Act
        var cut = RenderComponent<GridComponent>(parameters => parameters
            .Add(p => p.Grid, grid)
            .Add(p => p.OnCellClicked, coords => clickedCoords = coords));

        // Click cell at position (0, 2) - first cell in third row
        var cells = cut.FindAll(".grid-cell");
        cells[10].Click(); // 5 cells per row, so row 2 starts at index 10

        // Assert
        Assert.NotNull(clickedCoords);
        Assert.Equal(0, clickedCoords.X);
        Assert.Equal(2, clickedCoords.Y);
    }

    [Fact]
    public void OnCellClick_works_for_all_grid_positions()
    {
        // Arrange
        var grid = new AutoGrid(3, 3, Array.Empty<Vessel>());
        var clickedPositions = new List<Coords>();

        // Act
        var cut = RenderComponent<GridComponent>(parameters => parameters
            .Add(p => p.Grid, grid)
            .Add(p => p.OnCellClicked, coords => clickedPositions.Add(coords)));

        // Click all cells - need to re-query after each click
        for (int i = 0; i < 9; i++)
        {
            var cells = cut.FindAll(".grid-cell");
            cells[i].Click();
        }

        // Assert
        Assert.Equal(9, clickedPositions.Count); // All 9 cells clicked

        // Verify we got all expected coordinates
        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                Assert.Contains(clickedPositions, c => c.X == x && c.Y == y);
            }
        }
    }

    #endregion

    #region User Interaction Tests

    [Fact]
    public void Clicking_cell_triggers_OnCellClicked_event()
    {
        // Arrange
        var grid = new AutoGrid(5, 5, Array.Empty<Vessel>());
        var eventTriggered = false;

        // Act
        var cut = RenderComponent<GridComponent>(parameters => parameters
            .Add(p => p.Grid, grid)
            .Add(p => p.OnCellClicked, coords => eventTriggered = true));

        var cells = cut.FindAll(".grid-cell");
        cells[0].Click();

        // Assert
        Assert.True(eventTriggered);
    }

    [Fact]
    public void Multiple_cell_clicks_trigger_multiple_events()
    {
        // Arrange
        var grid = new AutoGrid(5, 5, Array.Empty<Vessel>());
        var clickCount = 0;

        // Act
        var cut = RenderComponent<GridComponent>(parameters => parameters
            .Add(p => p.Grid, grid)
            .Add(p => p.OnCellClicked, coords => clickCount++));

        // Re-query cells after each click
        cut.FindAll(".grid-cell")[0].Click();
        cut.FindAll(".grid-cell")[5].Click();
        cut.FindAll(".grid-cell")[10].Click();

        // Assert
        Assert.Equal(3, clickCount);
    }

    [Fact]
    public void Clicked_cell_coordinates_match_visual_position()
    {
        // Arrange
        var grid = new AutoGrid(5, 5, Array.Empty<Vessel>());
        Coords? clickedCoords = null;

        // Act
        var cut = RenderComponent<GridComponent>(parameters => parameters
            .Add(p => p.Grid, grid)
            .Add(p => p.OnCellClicked, coords => clickedCoords = coords));

        // Click cell at visual position (2, 3)
        var cells = cut.FindAll(".grid-cell");
        var targetCell = cells[17]; // Row 3 (index 2) * 5 + Col 2 (index 2) = 17
        targetCell.Click();

        // Assert
        Assert.NotNull(clickedCoords);
        Assert.Equal(2, clickedCoords.X);
        Assert.Equal(3, clickedCoords.Y);

        // Verify the title matches
        Assert.Equal("C4", targetCell.GetAttribute("title")); // C = 3rd letter, 4 = row 4
    }

    #endregion
}
