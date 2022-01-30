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
}