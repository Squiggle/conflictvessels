using ConflictVessels.Engine.Vessels;
using Xunit;

namespace ConflictVessels.Engine.Grids;

public class AutoGridTests
{
  [Fact]
  public void Creating_auto_grid_places_all_vessels()
  {
    var grid = new AutoGrid(new Vessel(3), new Vessel(2));
    Assert.True(grid.Ready);
    Assert.NotNull(grid.Vessels[0].Position);
    Assert.NotNull(grid.Vessels[1].Position);
  }
}