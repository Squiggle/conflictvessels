using System;
using System.Linq;
using ConflictVessels.Engine.Vessels;
using Xunit;

namespace ConflictVessels.Engine.Grids;

public class AutoGridTests
{
    [Fact]
    public void Creating_auto_grid_places_all_vessels()
    {
        var grid = new AutoGrid(10, 10, new Vessel(3), new Vessel(2));
        var ready = grid.Ready.Observe();
        Assert.True(ready.Value);
        Assert.NotNull(grid.Vessels[0].Position);
        Assert.NotNull(grid.Vessels[1].Position);
    }

    [Fact]
    public void Auto_grid_placements_are_randomised_and_unique()
    {
        var grid = new AutoGrid(10, 10,
          new Vessel(1),
          new Vessel(1),
          new Vessel(1),
          new Vessel(1)
        );

        var coords = grid.Vessels
          .SelectMany(v => v.Coords)
          .GroupBy(c => $"{c.X}:{c.Y}");
        Assert.All(
          coords.Select(c => c.Count()),
          c => Assert.Equal(1, c));
    }

    [Fact]
    public void Throws_exception_when_no_spaces_remain()
    {
        Assert.Throws<GridFullException>(
          () => new AutoGrid(1, 3, new Vessel(2), new Vessel(2))
        );
    }
}