using System;
using ConflictVessels.Engine.Grids;
using ConflictVessels.Engine.Vessels;
using Xunit;

namespace ConflictVessels.Engine.Tests.Grids;

public class GridTests
{
  [Fact]
  public void Grid_created_with_10x10()
  {
    var grid = Grid.Default();
    Assert.Equal(10, grid.Width);
    Assert.Equal(10, grid.Height);
  }

  [Fact]
  public void Default_grid_created_with_5_unplaced_vessels()
  {
    var grid = Grid.Default();
    Assert.Equal(5, grid.Vessels.Count);
    Assert.All(grid.Vessels, v => Assert.Null(v.Position));
  }

  [Fact]
  public void Placing_all_vessels_raises_ready_event()
  {
    // a basic grid with a tiny boat
    var grid = new Grid(10, 10, new Vessel(1));
    Assert.False(grid.Ready);

    var vessel = grid.Vessels[0].Vessel;

    var subscribedValue = false;
    grid.ObservableReady.Subscribe(x => subscribedValue = x);
    grid.Place(vessel, 0, 0, VesselOrientation.Vertical);
    Assert.True(grid.Ready);
    Assert.True(subscribedValue);
  }

  [Fact]
  public void Grid_supplies_list_of_all_coords()
  {
    var grid = new Grid(2, 2);
    Assert.Collection(grid.Coords(),
      c => Assert.Equal(new Coords(0, 0), c),
      c => Assert.Equal(new Coords(0, 1), c),
      c => Assert.Equal(new Coords(1, 0), c),
      c => Assert.Equal(new Coords(1, 1), c)
    );
  }
}