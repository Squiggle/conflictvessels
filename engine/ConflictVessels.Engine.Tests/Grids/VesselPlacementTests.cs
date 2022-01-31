using System.Linq;
using ConflictVessels.Engine.Grids;
using ConflictVessels.Engine.Vessels;
using Xunit;

namespace ConflictVessels.Engine.Tests.Grids;

public class VesselGridPlacementTests
{
  [Fact]
  public void VesselGridPlacement_is_created_without_a_position()
  {
    var vessel = new Vessel(2);
    var placement = new VesselGridPlacement(vessel);
    Assert.Null(placement.Position);
  }

  [Fact]
  public void Vessel_can_be_placed()
  {
    var vessel = new Vessel(2);
    var placement = new VesselGridPlacement(vessel);
    placement.Place(1, 1, VesselOrientation.Vertical);
    Assert.NotNull(placement.Position);
  }

  [Fact]
  public void Calculates_coords_for_long_vessel()
  {
    var vessel = new Vessel(6);
    var placement = new VesselGridPlacement(vessel);
    placement.Place(2, 1, VesselOrientation.Vertical);

    var coords = placement.Coords.ToList();
    Assert.Equal(6, coords.Count);
    Assert.All(placement.Coords, (coord) => Assert.Equal(2, coord.X));
    Assert.Equal(1, coords[0].Y);
    Assert.Equal(2, coords[1].Y);
    Assert.Equal(3, coords[2].Y);
    Assert.Equal(4, coords[3].Y);
    Assert.Equal(5, coords[4].Y);
    Assert.Equal(6, coords[5].Y);
  }
}