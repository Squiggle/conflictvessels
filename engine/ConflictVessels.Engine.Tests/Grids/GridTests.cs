using ConflictVessels.Engine.Grids;
using ConflictVessels.Engine.Vessels;
using System.Linq;
using Xunit;

namespace ConflictVessels.Engine.Tests.Grids;

public class GridTests
{
    [Fact]
    public void Grid_allows_placing_vessels()
    {
        var testVessel = new TestVessel();
        var grid = new Grid(10, 10, testVessel);
        grid.Place(testVessel, 0, 0, VesselOrientation.Horizontal);

        Assert.Same(testVessel, grid.Vessels.First().Vessel);
    }

    [Fact]
    public void Grid_is_only_ready_once_all_vessels_placed()
    {
        var testVessel = new TestVessel();
        var grid = new Grid(10, 10, testVessel);
        var ready = grid.Ready.Observe();

        Assert.False(ready.Value);
        grid.Place(testVessel, 0, 0, VesselOrientation.Horizontal);

        Assert.True(ready.Value);
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